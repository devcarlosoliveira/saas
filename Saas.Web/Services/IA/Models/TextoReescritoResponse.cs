using System.Text.Json.Serialization;

namespace Saas.Web.Services.IA.Models;

/// <summary>
/// Resposta da IA para reescrita do texto baseado nas tags selecionadas
/// </summary>
public class TextoReescritoResponse
{
    [JsonPropertyName("texto_reescrito")]
    public required string TextoReescrito { get; set; }

    [JsonPropertyName("explicacao")]
    public string? Explicacao { get; set; }

    [JsonPropertyName("tags_aplicadas")]
    public List<string> TagsAplicadas { get; set; } = [];
}
