using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Saas.Web.Data;
using Saas.Web.Models;
using Saas.Web.Models.Enums;
using Saas.Web.Services.IA;

namespace Saas.Web.Services.Document;

/// <summary>
/// Serviço orquestrador do processamento de documentos
/// Gerencia o fluxo completo: identificação de tags -> seleção do usuário -> reescrita
/// </summary>
public interface IContentProcessingService
{
    Task<Processamento> IniciarIdentificacaoTagsAsync(int documentoId, string usuarioId);
    Task<Processamento> ObterProcessamentoComTagsAsync(int processamentoId);
    Task<Processamento> SalvarTagsSelecionadasAsync(int processamentoId, List<int> tagsIds);
    Task<Processamento> ReescreverTextoAsync(int processamentoId);
    Task<List<Tag>> ObterTagsAtivasAsync();
}

public class ContentProcessingService : IContentProcessingService
{
    private readonly ApplicationDbContext _context;
    private readonly ISemanticKernelService _iaService;
    private readonly ILogger<ContentProcessingService> _logger;

    public ContentProcessingService(
        ApplicationDbContext context,
        ISemanticKernelService iaService,
        ILogger<ContentProcessingService> logger
    )
    {
        _context = context;
        _iaService = iaService;
        _logger = logger;
    }

    /// <summary>
    /// Etapa 1: Envia texto para IA identificar tags
    /// </summary>
    public async Task<Processamento> IniciarIdentificacaoTagsAsync(
        int documentoId,
        string usuarioId
    )
    {
        var documento = await _context.Documentos.FirstOrDefaultAsync(d =>
            d.Id == documentoId && d.UsuarioId == usuarioId
        );

        if (documento == null)
            throw new InvalidOperationException("Documento não encontrado");

        // Criar novo processamento
        var processamento = new Processamento
        {
            DocumentoId = documentoId,
            Tipo = ProcessamentoTipo.IdentificacaoTags,
            Status = ProcessamentoStatus.Processando,
            DataProcessamento = DateTime.UtcNow,
        };

        _context.Processamentos.Add(processamento);
        await _context.SaveChangesAsync();

        try
        {
            // Obter todas as tags ativas
            var tagsAtivas = await _context
                .Tags.Where(t => t.Ativo)
                .Select(t => t.Codigo)
                .ToListAsync();

            // Chamar IA para identificar tags
            var resposta = await _iaService.IdentificarTagsAsync(
                documento.TextoOriginal,
                tagsAtivas
            );

            // Salvar tags identificadas
            var tagsIdentificadas = new List<ProcessamentoTagIdentificada>();

            foreach (var tagIA in resposta.TagsIdentificadas)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Codigo == tagIA.Codigo);
                if (tag != null)
                {
                    tagsIdentificadas.Add(
                        new ProcessamentoTagIdentificada
                        {
                            ProcessamentoId = processamento.Id,
                            TagId = tag.Id,
                            Confianca = tagIA.Confianca,
                        }
                    );
                }
            }

            _context.AddRange(tagsIdentificadas);

            // Registrar execução do prompt
            var promptExecucao = new PromptExecucao
            {
                ProcessamentoId = processamento.Id,
                PromptTemplateId = 1, // Você pode criar templates específicos
                PromptEnviado =
                    $"Identificar tags em: {documento.TextoOriginal.Substring(0, Math.Min(100, documento.TextoOriginal.Length))}...",
                RespostaIa = JsonSerializer.Serialize(resposta),
                Metadados = JsonSerializer.Serialize(
                    new { modelo = "gpt-4o-mini", tipo = "identificacao" }
                ),
                DataExecucao = DateTime.UtcNow,
            };

            _context.PromptExecucoes.Add(promptExecucao);

            processamento.Status = ProcessamentoStatus.Concluido;
            processamento.TextoResultante = resposta.Analise ?? "Tags identificadas com sucesso";

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Processamento {ProcessamentoId} concluído. Tags identificadas: {Count}",
                processamento.Id,
                tagsIdentificadas.Count
            );

            return processamento;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar identificação de tags");
            processamento.Status = ProcessamentoStatus.Erro;
            processamento.TextoResultante = $"Erro: {ex.Message}";
            await _context.SaveChangesAsync();
            throw;
        }
    }

    /// <summary>
    /// Obter processamento com todas as tags (identificadas e selecionadas)
    /// </summary>
    public async Task<Processamento> ObterProcessamentoComTagsAsync(int processamentoId)
    {
        var processamento = await _context
            .Processamentos.Include(p => p.Documento)
            .Include(p => p.TagsIdentificadas)
                .ThenInclude(ti => ti.Tag)
            .Include(p => p.TagsSelecionadas)
                .ThenInclude(ts => ts.Tag)
            .FirstOrDefaultAsync(p => p.Id == processamentoId);

        if (processamento == null)
            throw new InvalidOperationException("Processamento não encontrado");

        return processamento;
    }

    /// <summary>
    /// Etapa 2: Usuário seleciona/modifica tags
    /// </summary>
    public async Task<Processamento> SalvarTagsSelecionadasAsync(
        int processamentoId,
        List<int> tagsIds
    )
    {
        var processamento = await _context
            .Processamentos.Include(p => p.TagsIdentificadas)
            .Include(p => p.TagsSelecionadas)
            .FirstOrDefaultAsync(p => p.Id == processamentoId);

        if (processamento == null)
            throw new InvalidOperationException("Processamento não encontrado");

        // Limpar seleções anteriores
        _context.RemoveRange(processamento.TagsSelecionadas);

        // Criar novas seleções
        var tagsIdentificadasIds = processamento.TagsIdentificadas.Select(ti => ti.TagId).ToList();

        foreach (var tagId in tagsIds)
        {
            var acaoUsuario = tagsIdentificadasIds.Contains(tagId)
                ? AcaoUsuario.Mantida
                : AcaoUsuario.Adicionada;

            processamento.TagsSelecionadas.Add(
                new ProcessamentoTagSelecionada
                {
                    ProcessamentoId = processamentoId,
                    TagId = tagId,
                    AcaoUsuario = acaoUsuario,
                }
            );
        }

        // Marcar tags removidas
        var tagsRemovidasIds = tagsIdentificadasIds.Except(tagsIds).ToList();
        foreach (var tagId in tagsRemovidasIds)
        {
            processamento.TagsSelecionadas.Add(
                new ProcessamentoTagSelecionada
                {
                    ProcessamentoId = processamentoId,
                    TagId = tagId,
                    AcaoUsuario = AcaoUsuario.Removida,
                }
            );
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Tags selecionadas salvas para processamento {ProcessamentoId}. Total: {Count}",
            processamentoId,
            tagsIds.Count
        );

        return processamento;
    }

    /// <summary>
    /// Etapa 3: Reescrever texto baseado nas tags selecionadas
    /// </summary>
    public async Task<Processamento> ReescreverTextoAsync(int processamentoId)
    {
        var processamentoAnterior = await _context
            .Processamentos.Include(p => p.Documento)
            .Include(p => p.TagsSelecionadas)
                .ThenInclude(ts => ts.Tag)
            .FirstOrDefaultAsync(p => p.Id == processamentoId);

        if (processamentoAnterior == null)
            throw new InvalidOperationException("Processamento não encontrado");

        // Criar novo processamento para a reescrita
        var novoProcessamento = new Processamento
        {
            DocumentoId = processamentoAnterior.DocumentoId,
            Tipo = ProcessamentoTipo.GeracaoConteudo,
            Status = ProcessamentoStatus.Processando,
            ProcessamentoAnteriorId = processamentoId,
            DataProcessamento = DateTime.UtcNow,
        };

        _context.Processamentos.Add(novoProcessamento);
        await _context.SaveChangesAsync();

        try
        {
            // Obter tags selecionadas (apenas as mantidas e adicionadas)
            var tagsSelecionadas = processamentoAnterior
                .TagsSelecionadas.Where(ts => ts.AcaoUsuario != AcaoUsuario.Removida)
                .Select(ts => ts.Tag!.Codigo)
                .ToList();

            if (!tagsSelecionadas.Any())
                throw new InvalidOperationException("Nenhuma tag selecionada para reescrita");

            // Chamar IA para reescrever
            var resposta = await _iaService.ReescreverTextoAsync(
                processamentoAnterior.Documento!.TextoOriginal,
                tagsSelecionadas
            );

            // Registrar execução do prompt
            var promptExecucao = new PromptExecucao
            {
                ProcessamentoId = novoProcessamento.Id,
                PromptTemplateId = 2, // Template de reescrita
                PromptEnviado = $"Reescrever texto com tags: {string.Join(", ", tagsSelecionadas)}",
                RespostaIa = JsonSerializer.Serialize(resposta),
                Metadados = JsonSerializer.Serialize(new { modelo = "gpt-4o", tipo = "reescrita" }),
                DataExecucao = DateTime.UtcNow,
            };

            _context.PromptExecucoes.Add(promptExecucao);

            novoProcessamento.Status = ProcessamentoStatus.Concluido;
            novoProcessamento.TextoResultante = resposta.TextoReescrito;

            // Atualizar documento
            processamentoAnterior.Documento.Status = DocumentoStatus.Concluido;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Reescrita concluída. Processamento {ProcessamentoId}",
                novoProcessamento.Id
            );

            return novoProcessamento;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao reescrever texto");
            novoProcessamento.Status = ProcessamentoStatus.Erro;
            novoProcessamento.TextoResultante = $"Erro: {ex.Message}";
            await _context.SaveChangesAsync();
            throw;
        }
    }

    /// <summary>
    /// Obter todas as tags ativas do catálogo
    /// </summary>
    public async Task<List<Tag>> ObterTagsAtivasAsync()
    {
        return await _context.Tags.Where(t => t.Ativo).OrderBy(t => t.Nome).ToListAsync();
    }
}
