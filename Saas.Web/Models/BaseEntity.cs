namespace Saas.Web.Models;

/// <summary>
/// Classe base com campos de auditoria
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Data de criação do registro
    /// </summary>
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Data da última atualização
    /// </summary>
    public DateTime? DataAtualizacao { get; set; }
}

/// <summary>
/// Classe base para entidades com soft delete
/// </summary>
public abstract class AuditableEntity : BaseEntity
{
    /// <summary>
    /// Indica se o registro está ativo (soft delete)
    /// </summary>
    public bool Ativo { get; set; } = true;

    /// <summary>
    /// Data de exclusão lógica
    /// </summary>
    public DateTime? DataExclusao { get; set; }
}
