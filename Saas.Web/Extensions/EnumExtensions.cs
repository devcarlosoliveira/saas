using System.ComponentModel;
using Saas.Web.Models.Enums;

namespace Saas.Web.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// Obtém a descrição de um enum
    /// </summary>
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field == null)
            return value.ToString();

        var attribute = (DescriptionAttribute?)
            Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
        return attribute?.Description ?? value.ToString();
    }

    /// <summary>
    /// Converte DocumentoStatus para string legível
    /// </summary>
    public static string ToFriendlyString(this DocumentoStatus status) =>
        status switch
        {
            DocumentoStatus.Processando => "Processando",
            DocumentoStatus.Concluido => "Concluído",
            DocumentoStatus.Erro => "Erro",
            DocumentoStatus.Pendente => "Pendente",
            _ => status.ToString(),
        };

    /// <summary>
    /// Converte ProcessamentoStatus para string legível
    /// </summary>
    public static string ToFriendlyString(this ProcessamentoStatus status) =>
        status switch
        {
            ProcessamentoStatus.Pendente => "Pendente",
            ProcessamentoStatus.Processando => "Processando",
            ProcessamentoStatus.Concluido => "Concluído",
            ProcessamentoStatus.Erro => "Erro",
            _ => status.ToString(),
        };

    /// <summary>
    /// Converte ProcessamentoTipo para string legível
    /// </summary>
    public static string ToFriendlyString(this ProcessamentoTipo tipo) =>
        tipo switch
        {
            ProcessamentoTipo.IdentificacaoTags => "Identificação de Tags",
            ProcessamentoTipo.GeracaoConteudo => "Geração de Conteúdo",
            ProcessamentoTipo.Resumo => "Resumo",
            ProcessamentoTipo.Expansao => "Expansão",
            ProcessamentoTipo.Revisao => "Revisão",
            ProcessamentoTipo.Traducao => "Tradução",
            ProcessamentoTipo.OtimizacaoSEO => "Otimização SEO",
            _ => tipo.ToString(),
        };

    /// <summary>
    /// Converte AcaoUsuario para string legível
    /// </summary>
    public static string ToFriendlyString(this AcaoUsuario acao) =>
        acao switch
        {
            AcaoUsuario.Mantida => "Mantida",
            AcaoUsuario.Adicionada => "Adicionada",
            AcaoUsuario.Removida => "Removida",
            _ => acao.ToString(),
        };

    /// <summary>
    /// Verifica se o status indica processamento em andamento
    /// </summary>
    public static bool IsProcessando(this DocumentoStatus status) =>
        status == DocumentoStatus.Processando;

    /// <summary>
    /// Verifica se o status indica conclusão
    /// </summary>
    public static bool IsConcluido(this DocumentoStatus status) =>
        status == DocumentoStatus.Concluido;

    /// <summary>
    /// Verifica se o status indica erro
    /// </summary>
    public static bool IsErro(this DocumentoStatus status) => status == DocumentoStatus.Erro;

    /// <summary>
    /// Verifica se o processamento pode ser cancelado
    /// </summary>
    public static bool PodeCancelar(this ProcessamentoStatus status) =>
        status == ProcessamentoStatus.Pendente || status == ProcessamentoStatus.Processando;

    /// <summary>
    /// Verifica se o processamento foi finalizado (com sucesso ou erro)
    /// </summary>
    public static bool IsFinalizado(this ProcessamentoStatus status) =>
        status == ProcessamentoStatus.Concluido || status == ProcessamentoStatus.Erro;
}
