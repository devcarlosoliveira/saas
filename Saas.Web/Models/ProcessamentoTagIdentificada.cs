using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Web.Models;

[Table("ProcessamentoTagIdentificada")]
public class ProcessamentoTagIdentificada
{
    public int ProcessamentoId { get; set; }
    public int TagId { get; set; }

    [Range(0, 1)]
    public decimal? Confianca { get; set; } // Score da IA (0.0 a 1.0)

    // Navegação
    public virtual Processamento? Processamento { get; set; }
    public virtual Tag? Tag { get; set; }
}
