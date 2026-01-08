namespace Saas.Web.Models.Enums;

/// <summary>
/// Tipos de processamento disponíveis
/// </summary>
public enum ProcessamentoTipo
{
    /// <summary>
    /// Identificação de tags/categorias no texto
    /// </summary>
    IdentificacaoTags = 0,

    /// <summary>
    /// Geração de conteúdo baseado em template
    /// </summary>
    GeracaoConteudo = 1,

    /// <summary>
    /// Resumo do texto
    /// </summary>
    Resumo = 2,

    /// <summary>
    /// Expansão do conteúdo
    /// </summary>
    Expansao = 3,

    /// <summary>
    /// Revisão ortográfica e gramatical
    /// </summary>
    Revisao = 4,

    /// <summary>
    /// Tradução de texto
    /// </summary>
    Traducao = 5,

    /// <summary>
    /// Otimização para SEO
    /// </summary>
    OtimizacaoSEO = 6,
}
