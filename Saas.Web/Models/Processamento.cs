using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Saas.Web.Models.Enums;

namespace Saas.Web.Models;

public class Processamento
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Documento")]
    public int DocumentoId { get; set; }

    public ProcessamentoTipo Tipo { get; set; }

    public DateTime DataProcessamento { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "TEXT")]
    public string TextoResultante { get; set; } = string.Empty;

    public ProcessamentoStatus Status { get; set; } = ProcessamentoStatus.Pendente;

    // Self-reference para versões anteriores
    [ForeignKey("ProcessamentoAnterior")]
    public int? ProcessamentoAnteriorId { get; set; }

    // Navegação
    public virtual Documento? Documento { get; set; }
    public virtual Processamento? ProcessamentoAnterior { get; set; }
    public virtual ICollection<Processamento> ProcessamentosPosteriores { get; set; } = [];
    public virtual ICollection<ProcessamentoTagIdentificada> TagsIdentificadas { get; set; } = [];
    public virtual ICollection<ProcessamentoTagSelecionada> TagsSelecionadas { get; set; } = [];
    public virtual ICollection<PromptExecucao> PromptExecucoes { get; set; } = [];
}
