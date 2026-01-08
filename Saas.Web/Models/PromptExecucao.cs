using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saas.Web.Models;

public class PromptExecucao
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Processamento")]
    public int ProcessamentoId { get; set; }

    [Required]
    [ForeignKey("PromptTemplate")]
    public int PromptTemplateId { get; set; }

    [Required]
    [Column(TypeName = "TEXT")]
    public required string PromptEnviado { get; set; } // Com placeholders preenchidos

    [Required]
    [Column(TypeName = "TEXT")]
    public required string RespostaIa { get; set; } // Resposta bruta da IA

    //[Column(TypeName = "jsonb")] // Para PostgreSQL
    [Column(TypeName = "TEXT")]
    public string Metadados { get; set; } = string.Empty; // JSON: {"modelo": "gpt-4", "tokens": 450}

    public DateTime DataExecucao { get; set; } = DateTime.UtcNow;

    // Navegação
    public virtual Processamento? Processamento { get; set; }
    public virtual PromptTemplate? PromptTemplate { get; set; }
}
