using System.Text.Json.Serialization;

namespace Saas.Web.Services.IA.Models;

/// <summary>
/// Resposta da IA para identificação de tags no texto
/// </summary>
public class TagIdentificacaoResponse
{
    [JsonPropertyName("tags_identificadas")]
    public List<TagIdentificada> TagsIdentificadas { get; set; } = [];

    [JsonPropertyName("analise")]
    public string? Analise { get; set; }
}

public class TagIdentificada
{
    [JsonPropertyName("codigo")]
    public required string Codigo { get; set; }

    [JsonPropertyName("confianca")]
    public decimal Confianca { get; set; }

    [JsonPropertyName("justificativa")]
    public string? Justificativa { get; set; }
}
