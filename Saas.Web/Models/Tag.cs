using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Web.Models;

public class Tag
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Codigo { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Nome { get; set; }

    [Column(TypeName = "TEXT")]
    public string? Descricao { get; set; }

    public bool Ativo { get; set; } = true;

    // Navegação
    public virtual ICollection<ProcessamentoTagIdentificada> ProcessamentosIdentificados { get; set; } =
    [];
    public virtual ICollection<ProcessamentoTagSelecionada> ProcessamentosSelecionados { get; set; } =
    [];
}
