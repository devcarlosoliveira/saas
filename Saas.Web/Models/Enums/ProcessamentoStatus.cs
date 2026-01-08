namespace Saas.Web.Models.Enums;

/// <summary>
/// Status de um processamento específico
/// </summary>
public enum ProcessamentoStatus
{
    /// <summary>
    /// Processamento aguardando execução
    /// </summary>
    Pendente = 0,

    /// <summary>
    /// Processamento em execução
    /// </summary>
    Processando = 1,

    /// <summary>
    /// Processamento concluído com sucesso
    /// </summary>
    Concluido = 2,

    /// <summary>
    /// Erro durante o processamento
    /// </summary>
    Erro = 3,
}
