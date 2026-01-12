using Saas.Web.Data;
using Saas.Web.Models;

namespace Saas.Web.Data.Seeds;

public static class TagSeeder
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
                Codigo = "dc01-lista-ilustrada",
                Nome = "Lista Ilustrada",
                Descricao = "Enumeração de itens onde cada ponto é acompanhado de um ícone ou elemento visual que reforça o conceito.", 
                Ativo = true,
            },
            new Tag
            {
                Codigo = "dc02-checklist",
                Nome = "Checklist",
                Descricao = "Lista de tarefas ou requisitos com caixas de seleção, focada em conclusão ou verificação de passos.",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "dc03-mini-faq",
                Nome = "Mini FAQ",
                Descricao = "Seção de perguntas frequentes e respostas diretas sobre um tópico específico do texto.",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "dc04-comparacao",
                Nome = "Comparação A x B",
                Descricao = "Confronto direto entre dois conceitos, produtos ou ideias, destacando diferenças e semelhanças.",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "dc05-pros-x-contras",
                Nome = "Prós x Contras",
                Descricao = "Quadro equilibrado apresentando as vantagens e desvantagens de uma escolha ou situação.",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "dc06-melhor-escolha",
                Nome = "Melhor Escolha",
                Descricao = "Destaque editorial indicando a opção mais recomendada entre várias apresentadas.",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "dc07-passo-a-passo",
                Nome = "Passo a Passo",
                Descricao = "Guia sequencial numerado que orienta o leitor através de uma execução técnica ou prática.",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "dc08-linha-do-tempo",
                Nome = "Linha do Tempo",
                Descricao = "Organização cronológica de eventos, marcos históricos ou evolução de um processo.",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "dc09-causas-consequencias",
                Nome = "Causas e Consequências",
                Descricao = "Explicação lógica que conecta um evento (gatilho) aos seus resultados diretos.",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "dc10-galeria-de-imagens",
                Nome = "Galeria de Imagens",
                Descricao = "Agrupamento visual de fotos, ilustrações ou prints que servem de apoio ao conteúdo textual.",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "dc11-aspas",
                Nome = "Aspas",
                Descricao = "Destaque visual (pull quote) para frases de impacto, depoimentos ou citações de autoridades.",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "dc12-selos-e-logos",
                Nome = "Selos e Logos",
                Descricao = "Inclusão de elementos gráficos de certificação, marcas parceiras ou selos de garantia.",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "dc13-mini-bio",
                Nome = "Mini Bio",
                Descricao = "Breve perfil biográfico (quem é) sobre o autor, personagem ou especialista mencionado.",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "dc14-numeros-ilustrados",
                Nome = "Números Ilustrados",
                Descricao = "Estatísticas ou dados quantitativos destacados com fontes grandes e ícones explicativos.",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "dc15-grafico",
                Nome = "Gráfico",
                Descricao = "Representação visual de dados estatísticos (barras, pizza, linhas) para facilitar a interpretação.",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "dc16-tabela-associativa",
                Nome = "Tabela Associativa",
                Descricao = "Grade de dados que relaciona diferentes categorias em colunas e linhas.",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "dc17-diagrama",
                Nome = "Diagrama",
                Descricao = "Desenho esquemático que explica como algo funciona ou como partes de um todo se conectam.",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "dc18-fluxograma",
                Nome = "Fluxograma",
                Descricao = "Representação gráfica de um processo de decisão ou fluxo de trabalho com início, meio e fim.",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "dc19-mapa-ilustrado",
                Nome = "Mapa Ilustrado",
                Descricao = "Representação geográfica estilizada indicando locais, rotas ou dados regionais.",
                Ativo = true,
            },
            new Tag
            {
                Codigo = "dc20-infografico",
                Nome = "Infográfico",
                Descricao = "Peça complexa que combina texto curto com diversos elementos visuais para explicar um tema de forma rápida.",
                Ativo = true,
            },
        };

        context.Tags.AddRange(tags);
        await context.SaveChangesAsync();
    }
}
