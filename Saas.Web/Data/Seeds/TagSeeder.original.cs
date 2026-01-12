using Saas.Web.Data;
using Saas.Web.Models;

namespace Saas.Web.Data.Seeds;

public static class TagSeederOriginal
{
    public static async Task SeedTagsAsync(ApplicationDbContext context)
    {
        if (context.Tags.Any())
        {
            return; // Já existe tags no banco
        }

        var tags = new List<Tag>
        {
            new Tag
            {
                Codigo = "dissertacao",
                Nome = "Dissertação",
                Descricao =
                    "Texto dissertativo com estrutura argumentativa (introdução, desenvolvimento, conclusão)",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "citacao",
                Nome = "Citação",
                Descricao = "Conteúdo com citações de autores, estudos ou fontes",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "tabela",
                Nome = "Tabela Associativa",
                Descricao = "Informações organizadas em formato de tabela com linhas e colunas",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "grafico",
                Nome = "Gráfico",
                Descricao = "Dados que podem ser representados visualmente em gráficos",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "infografico",
                Nome = "Infográfico",
                Descricao = "Conteúdo visual com combinação de texto, ícones e dados",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "lista",
                Nome = "Lista",
                Descricao = "Conteúdo em formato de lista numerada ou com marcadores",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "comparacao",
                Nome = "Comparação",
                Descricao = "Comparativo entre dois ou mais elementos, conceitos ou produtos",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "biografia",
                Nome = "Biografia",
                Descricao = "História ou perfil de uma pessoa, empresa ou entidade",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "diagrama",
                Nome = "Diagrama",
                Descricao = "Fluxo, processo ou estrutura que pode ser representada visualmente",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "faq",
                Nome = "FAQ",
                Descricao = "Perguntas frequentes com respostas diretas",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "tutorial",
                Nome = "Tutorial",
                Descricao = "Guia passo a passo ou instruções de como fazer algo",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "resumo",
                Nome = "Resumo",
                Descricao = "Síntese ou resumo executivo do conteúdo",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "narrativa",
                Nome = "Narrativa",
                Descricao = "Texto com estrutura narrativa (personagens, enredo, tempo, espaço)",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "definicao",
                Nome = "Definição",
                Descricao = "Definições de conceitos, termos ou glossário",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "linha_tempo",
                Nome = "Linha do Tempo",
                Descricao = "Eventos organizados cronologicamente",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "estudo_caso",
                Nome = "Estudo de Caso",
                Descricao = "Análise detalhada de um caso específico ou exemplo prático",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "estatisticas",
                Nome = "Estatísticas",
                Descricao = "Dados numéricos, percentuais ou estatísticos",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "pros_contras",
                Nome = "Prós e Contras",
                Descricao = "Vantagens e desvantagens de algo",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "checklist",
                Nome = "Checklist",
                Descricao = "Lista de verificação ou itens a serem checados",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "mapa_mental",
                Nome = "Mapa Mental",
                Descricao = "Ideias centrais com ramificações e conexões",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "exemplo",
                Nome = "Exemplo Prático",
                Descricao = "Exemplos concretos e práticos do conteúdo",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "metafora",
                Nome = "Metáfora/Analogia",
                Descricao = "Explicações usando metáforas ou analogias",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "dica",
                Nome = "Dicas",
                Descricao = "Dicas práticas e conselhos",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "alerta",
                Nome = "Alertas/Avisos",
                Descricao = "Avisos importantes, cuidados ou alertas",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "card",
                Nome = "Cards",
                Descricao = "Conteúdo estruturado em cards ou caixas informativas",
                Ativo = true,
            },
        };

        context.Tags.AddRange(tags);
        await context.SaveChangesAsync();
    }
}
