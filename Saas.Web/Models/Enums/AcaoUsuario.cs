namespace Saas.Web.Models.Enums;

/// <summary>
/// Ação do usuário em relação a uma tag
/// </summary>
public enum AcaoUsuario
{
    /// <summary>
    /// Tag foi mantida como sugerida pela IA
    /// </summary>
    Mantida = 0,

    /// <summary>
    /// Tag foi adicionada manualmente pelo usuário
    /// </summary>
    Adicionada = 1,

    /// <summary>
    /// Tag foi removida pelo usuário
    /// </summary>
    Removida = 2,
}
