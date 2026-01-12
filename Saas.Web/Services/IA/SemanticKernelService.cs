using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Saas.Web.Services.IA.Models;

namespace Saas.Web.Services.IA;

/// <summary>
/// Serviço que encapsula a comunicação com OpenAI usando Semantic Kernel
/// </summary>
public interface ISemanticKernelService
{
    Task<TagIdentificacaoResponse> IdentificarTagsAsync(
        string textoOriginal,
        List<string> tagsCatalogo
    );
    Task<TextoReescritoResponse> ReescreverTextoAsync(
        string textoOriginal,
        List<string> tagsSelecionadas
    );
}

public class SemanticKernelService : ISemanticKernelService
{
    private readonly Kernel _kernel;
    private readonly ILogger<SemanticKernelService> _logger;
    private readonly string _modeloIdentificacao;
    private readonly string _modeloReescrita;

    public SemanticKernelService(
        IConfiguration configuration,
        ILogger<SemanticKernelService> logger
    )
    {
        _logger = logger;

        var apiKey =
            configuration["OpenAI:ApiKey"]
            ?? throw new ArgumentNullException("OpenAI:ApiKey não configurada");

        _modeloIdentificacao = configuration["OpenAI:ModeloIdentificacao"] ?? "gpt-5-nano";
        _modeloReescrita = configuration["OpenAI:ModeloReescrita"] ?? "gpt-5-nano";

        // Criar Kernel com OpenAI
        var builder = Kernel.CreateBuilder();
        builder.AddOpenAIChatCompletion(modelId: _modeloIdentificacao, apiKey: apiKey);

        _kernel = builder.Build();

        _logger.LogInformation(
            "SemanticKernelService inicializado com modelo: {Modelo}",
            _modeloIdentificacao
        );
    }

    /// <summary>
    /// Identifica tags/padrões no texto usando IA
    /// </summary>
    public async Task<TagIdentificacaoResponse> IdentificarTagsAsync(
        string textoOriginal,
        List<string> tagsCatalogo
    )
    {
        try
        {
            var chatService = _kernel.GetRequiredService<IChatCompletionService>();

            var prompt = $$$"""
                Você é um especialista em análise de conteúdo educacional e técnico.

                Analise o texto abaixo e identifique quais dos seguintes padrões/categorias estão presentes:

                TAGS DISPONÍVEIS:
                {string.Join(", ", tagsCatalogo)}

                TEXTO PARA ANÁLISE:
                {textoOriginal}

                INSTRUÇÕES:
                1. Identifique TODAS as tags que se aplicam ao texto
                2. Para cada tag identificada, forneça:
                   - codigo: o código exato da tag (deve corresponder a uma das tags disponíveis)
                   - confianca: um valor entre 0.0 e 1.0 indicando sua certeza
                   - justificativa: breve explicação do porquê identificou essa tag
                3. Retorne APENAS um JSON válido no formato especificado

                FORMATO DE RESPOSTA (JSON):
                {{
                  "tags_identificadas": [
                    {{
                      "codigo": "dissertacao",
                      "confianca": 0.95,
                      "justificativa": "O texto apresenta estrutura dissertativa com introdução, desenvolvimento e conclusão"
                    }}
                  ],
                  "analise": "Análise geral do texto e justificativa das escolhas"
                }}

                RESPONDA APENAS COM O JSON, SEM TEXTO ADICIONAL:
                """;

            var executionSettings = new OpenAIPromptExecutionSettings
            {
                Temperature = 0.3,
                MaxTokens = 2000,
                ResponseFormat = "json_object", // Force JSON response
            };

            var resultado = await chatService.GetChatMessageContentAsync(
                prompt,
                executionSettings,
                _kernel
            );

            var jsonResponse = resultado.Content ?? "{}";

            _logger.LogInformation(
                "IA - Identificação de tags concluída. Resposta: {Response}",
                jsonResponse.Substring(0, Math.Min(200, jsonResponse.Length))
            );

            var response =
                JsonSerializer.Deserialize<TagIdentificacaoResponse>(
                    jsonResponse,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                ) ?? new TagIdentificacaoResponse();

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao identificar tags com IA");
            throw new ApplicationException("Falha ao processar identificação de tags", ex);
        }
    }

    /// <summary>
    /// Reescreve o texto aplicando as tags selecionadas
    /// </summary>
    public async Task<TextoReescritoResponse> ReescreverTextoAsync(
        string textoOriginal,
        List<string> tagsSelecionadas
    )
    {
        try
        {
            var chatService = _kernel.GetRequiredService<IChatCompletionService>();

            var prompt = $$$"""
                Você é um redator especializado em transformar e adaptar conteúdos educacionais.

                Reescreva o texto abaixo aplicando os seguintes formatos/estilos:

                FORMATOS A APLICAR:
                {string.Join(", ", tagsSelecionadas)}

                TEXTO ORIGINAL:
                {textoOriginal}

                INSTRUÇÕES:
                1. Mantenha o conteúdo essencial do texto original
                2. Adapte a estrutura e apresentação conforme os formatos solicitados
                3. Se solicitado "tabela", organize informações em formato de tabela
                4. Se solicitado "lista", estruture em tópicos ou lista numerada
                5. Se solicitado "infografico", estruture de forma visual com títulos, subtítulos e bullets
                6. Se solicitado "comparacao", organize em formato comparativo
                7. Se solicitado "faq", transforme em perguntas e respostas
                8. Combine múltiplos formatos se necessário
                9. Retorne APENAS um JSON válido

                FORMATO DE RESPOSTA (JSON):
                {{
                  "texto_reescrito": "Texto completo reescrito aplicando os formatos",
                  "explicacao": "Breve explicação das transformações aplicadas",
                  "tags_aplicadas": ["tag1", "tag2"]
                }}

                RESPONDA APENAS COM O JSON, SEM TEXTO ADICIONAL:
                """;

            var executionSettings = new OpenAIPromptExecutionSettings
            {
                Temperature = 0.7,
                MaxTokens = 4000,
                ResponseFormat = "json_object",
            };

            var resultado = await chatService.GetChatMessageContentAsync(
                prompt,
                executionSettings,
                _kernel
            );

            var jsonResponse = resultado.Content ?? "{}";

            _logger.LogInformation(
                "IA - Reescrita concluída. Tamanho da resposta: {Size} chars",
                jsonResponse.Length
            );

            var response =
                JsonSerializer.Deserialize<TextoReescritoResponse>(
                    jsonResponse,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                ) ?? new TextoReescritoResponse { TextoReescrito = textoOriginal };

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao reescrever texto com IA");
            throw new ApplicationException("Falha ao reescrever texto", ex);
        }
    }
}
