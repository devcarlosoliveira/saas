namespace Saas.Web.Models.Enums;

/// <summary>
/// Status do processamento de um documento
/// </summary>
public enum DocumentoStatus
{
    /// <summary>
    /// Documento está sendo processado
    /// </summary>
    Processando = 0,

    /// <summary>
    /// Documento processado com sucesso
    /// </summary>
    Concluido = 1,

    /// <summary>
    /// Erro no processamento do documento
    /// </summary>
    Erro = 2,

    /// <summary>
    /// Documento aguardando processamento
    /// </summary>
    Pendente = 3,
}
