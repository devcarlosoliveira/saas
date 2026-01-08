using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Web.Models;

public class PromptTemplate
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Nome { get; set; }

    [Column(TypeName = "TEXT")]
    public string? Descricao { get; set; }

    [Required]
    [Column(TypeName = "TEXT")]
    public required string Template { get; set; }

    // [Column(TypeName = "nvarchar(max)")] // Para SQL Server JSON
    [Column(TypeName = "TEXT")]
    public string? Parametros { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public bool Ativo { get; set; } = true;

    // Criador (opcional)
    [ForeignKey("Usuario")]
    public string? CriadoPor { get; set; }

    // Navegação
    public virtual ApplicationUser? UsuarioCriador { get; set; }
    public virtual ICollection<PromptExecucao> Execucoes { get; set; } = [];
}
