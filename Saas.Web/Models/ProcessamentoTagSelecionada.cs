using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Saas.Web.Models.Enums;

namespace Saas.Web.Models;

[Table("ProcessamentoTagSelecionada")]
public class ProcessamentoTagSelecionada
{
    public int ProcessamentoId { get; set; }
    public int TagId { get; set; }

    public AcaoUsuario AcaoUsuario { get; set; } = AcaoUsuario.Mantida;

    // Navegação
    public virtual Processamento? Processamento { get; set; }
    public virtual Tag? Tag { get; set; }
}
