using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saas.Web.Data;
using Saas.Web.Models;
using Saas.Web.Models.Enums;
using Saas.Web.Services.Document;

namespace Saas.Web.Controllers;

[Authorize]
public class DocumentoController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IContentProcessingService _processingService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<DocumentoController> _logger;

    public DocumentoController(
        ApplicationDbContext context,
        IContentProcessingService processingService,
        UserManager<ApplicationUser> userManager,
        ILogger<DocumentoController> logger
    )
    {
        _context = context;
        _processingService = processingService;
        _userManager = userManager;
        _logger = logger;
    }

    // GET: /Documento
    // Lista todos os documentos do usuário
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);

        var documentos = await _context
            .Documentos.Where(d => d.UsuarioId == userId)
            .OrderByDescending(d => d.DataCriacao)
            .Include(d => d.Processamentos)
            .ToListAsync();

        return View(documentos);
    }

    // GET: /Documento/Novo
    // Formulário para enviar novo texto
    public async Task<IActionResult> Novo()
    {
        var tags = await _processingService.ObterTagsAtivasAsync();
        return View(tags);
    }

    // POST: /Documento/Novo
    // Recebe texto e inicia identificação de tags
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Novo(string textoOriginal, string? titulo)
    {
        if (string.IsNullOrWhiteSpace(textoOriginal))
        {
            ModelState.AddModelError("textoOriginal", "O texto é obrigatório");
            return View();
        }

        if (textoOriginal.Length < 50)
        {
            ModelState.AddModelError("textoOriginal", "O texto deve ter pelo menos 50 caracteres");
            return View();
        }

        try
        {
            var userId = _userManager.GetUserId(User)!;

            // Criar documento
            var documento = new Documento
            {
                UsuarioId = userId,
                TextoOriginal = textoOriginal,
                Titulo = titulo,
                Status = DocumentoStatus.Processando,
                DataCriacao = DateTime.UtcNow,
            };

            _context.Documentos.Add(documento);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Documento {DocumentoId} criado por usuário {UserId}",
                documento.Id,
                userId
            );

            // Iniciar processamento de identificação de tags (assíncrono)
            var processamento = await _processingService.IniciarIdentificacaoTagsAsync(
                documento.Id,
                userId
            );

            TempData["Sucesso"] = "Texto enviado! Identificando padrões...";

            return RedirectToAction(nameof(SelecionarTags), new { id = processamento.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar documento");
            ModelState.AddModelError("", "Erro ao processar seu texto. Tente novamente.");
            return View();
        }
    }

    // GET: /Documento/SelecionarTags/5
    // Mostra tags identificadas e permite seleção
    public async Task<IActionResult> SelecionarTags(int id)
    {
        try
        {
            var userId = _userManager.GetUserId(User);
            var processamento = await _processingService.ObterProcessamentoComTagsAsync(id);

            // Verificar se usuário é dono do documento
            if (processamento.Documento?.UsuarioId != userId)
                return Forbid();

            // Se ainda está processando, mostrar página de loading
            if (processamento.Status == ProcessamentoStatus.Processando)
            {
                return View("Processando", processamento);
            }

            if (processamento.Status == ProcessamentoStatus.Erro)
            {
                TempData["Erro"] = "Erro ao identificar tags: " + processamento.TextoResultante;
                return RedirectToAction(nameof(Index));
            }

            // Carregar todas as tags disponíveis
            var todasTags = await _processingService.ObterTagsAtivasAsync();

            ViewBag.TodasTags = todasTags;
            ViewBag.TagsIdentificadas = processamento
                .TagsIdentificadas.OrderByDescending(ti => ti.Confianca)
                .ToList();

            return View(processamento);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar seleção de tags");
            TempData["Erro"] = "Erro ao carregar tags.";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: /Documento/SelecionarTags/5
    // Salva tags selecionadas e inicia reescrita
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SelecionarTags(int id, List<int> tagsSelecionadas)
    {
        if (!tagsSelecionadas.Any())
        {
            TempData["Erro"] = "Selecione pelo menos uma tag";
            return RedirectToAction(nameof(SelecionarTags), new { id });
        }

        try
        {
            var userId = _userManager.GetUserId(User);
            var processamento = await _context
                .Processamentos.Include(p => p.Documento)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (processamento?.Documento?.UsuarioId != userId)
                return Forbid();

            // Salvar tags selecionadas
            await _processingService.SalvarTagsSelecionadasAsync(id, tagsSelecionadas);

            // Iniciar reescrita
            var processamentoReescrita = await _processingService.ReescreverTextoAsync(id);

            TempData["Sucesso"] = "Texto reescrito com sucesso!";

            return RedirectToAction(nameof(Resultado), new { id = processamentoReescrita.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar tags selecionadas");
            TempData["Erro"] = "Erro ao reescrever texto. Tente novamente.";
            return RedirectToAction(nameof(SelecionarTags), new { id });
        }
    }

    // GET: /Documento/Resultado/7
    // Mostra texto reescrito
    public async Task<IActionResult> Resultado(int id)
    {
        try
        {
            var userId = _userManager.GetUserId(User);

            var processamento = await _context
                .Processamentos.Include(p => p.Documento)
                .Include(p => p.ProcessamentoAnterior)
                    .ThenInclude(pa => pa!.TagsSelecionadas)
                        .ThenInclude(ts => ts.Tag)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (processamento?.Documento?.UsuarioId != userId)
                return Forbid();

            if (processamento?.Status == ProcessamentoStatus.Processando)
            {
                return View("Processando", processamento);
            }

            return View(processamento);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar resultado");
            TempData["Erro"] = "Erro ao carregar resultado.";
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: /Documento/Detalhes/3
    // Visualiza documento e histórico de processamentos
    public async Task<IActionResult> Detalhes(int id)
    {
        var userId = _userManager.GetUserId(User);

        var documento = await _context
            .Documentos.Include(d => d.Processamentos)
                .ThenInclude(p => p.TagsIdentificadas)
                    .ThenInclude(ti => ti.Tag)
            .Include(d => d.Processamentos)
                .ThenInclude(p => p.TagsSelecionadas)
                    .ThenInclude(ts => ts.Tag)
            .FirstOrDefaultAsync(d => d.Id == id && d.UsuarioId == userId);

        if (documento == null)
            return NotFound();

        return View(documento);
    }

    // POST: /Documento/Excluir/3
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Excluir(int id)
    {
        var userId = _userManager.GetUserId(User);

        var documento = await _context.Documentos.FirstOrDefaultAsync(d =>
            d.Id == id && d.UsuarioId == userId
        );

        if (documento == null)
            return NotFound();

        try
        {
            _context.Documentos.Remove(documento);
            await _context.SaveChangesAsync();

            TempData["Sucesso"] = "Documento excluído com sucesso";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir documento {DocumentoId}", id);
            TempData["Erro"] = "Erro ao excluir documento";
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: /Documento/ReprocessarTags/5
    // Reprocessa identificação de tags
    public async Task<IActionResult> ReprocessarTags(int id)
    {
        try
        {
            var userId = _userManager.GetUserId(User)!;

            var documento = await _context.Documentos.FirstOrDefaultAsync(d =>
                d.Id == id && d.UsuarioId == userId
            );

            if (documento == null)
                return NotFound();

            var novoProcessamento = await _processingService.IniciarIdentificacaoTagsAsync(
                id,
                userId
            );

            TempData["Sucesso"] = "Reprocessando identificação de tags...";
            return RedirectToAction(nameof(SelecionarTags), new { id = novoProcessamento.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao reprocessar documento {DocumentoId}", id);
            TempData["Erro"] = "Erro ao reprocessar documento";
            return RedirectToAction(nameof(Index));
        }
    }
}
