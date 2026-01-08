using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Saas.Web.Models.Enums;

namespace Saas.Web.Models;

public class Documento
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Usuario")]
    public required string UsuarioId { get; set; }

    [Required]
    [Column(TypeName = "TEXT")]
    public required string TextoOriginal { get; set; }

    [MaxLength(200)]
    public string? Titulo { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public DocumentoStatus Status { get; set; } = DocumentoStatus.Processando;

    // Navegação
    public virtual ApplicationUser? Usuario { get; set; }
    public virtual ICollection<Processamento> Processamentos { get; set; } = [];
}
