# 🤖 Sistema de Reescrita de Conteúdo com IA

## 📋 Visão Geral da Implementação

Este documento descreve a implementação completa do sistema de reescrita de conteúdo usando IA com **Microsoft Semantic Kernel**.

## 🏗️ Arquitetura Implementada

### **1. Camada de Modelos (Models/)**

#### Entidades Principais:
- **Documento**: Armazena o texto original do usuário
- **Tag**: Catálogo de 25+ padrões (dissertação, lista, FAQ, tabela, etc.)
- **Processamento**: Representa cada etapa do fluxo (identificação ou reescrita)
- **ProcessamentoTagIdentificada**: Tags identificadas pela IA com score de confiança
- **ProcessamentoTagSelecionada**: Tags que o usuário manteve/modificou
- **PromptTemplate & PromptExecucao**: Rastreamento completo de interações com IA

### **2. Camada de Serviços (Services/)**

#### **SemanticKernelService** (Services/IA/)
- **Por que Semantic Kernel?**
  - ✅ Integração nativa com OpenAI
  - ✅ Desenvolvido pela Microsoft para .NET
  - ✅ Mais simples que AutoGen (que é para agentes autônomos)
  - ✅ Suporte a prompts estruturados e JSON
  - ✅ Melhor que ML.NET (que é para treinar modelos próprios)

- **Métodos Implementados:**
  - `IdentificarTagsAsync()`: Envia texto para GPT-4o-mini identificar padrões
  - `ReescreverTextoAsync()`: Usa GPT-4o para reescrever com base nas tags selecionadas

#### **ContentProcessingService** (Services/Document/)
- Orquestra todo o fluxo de processamento
- Gerencia transações e persistência no banco
- **Métodos:**
  - `IniciarIdentificacaoTagsAsync()`: Etapa 1 - Identificação
  - `SalvarTagsSelecionadasAsync()`: Etapa 2 - Seleção do usuário
  - `ReescreverTextoAsync()`: Etapa 3 - Reescrita final

### **3. Camada de Controladores (Controllers/)**

#### **DocumentoController**
- **Endpoints:**
  - `GET /Documento` - Lista documentos do usuário
  - `GET /Documento/Novo` - Formulário para novo texto
  - `POST /Documento/Novo` - Cria documento e inicia identificação
  - `GET /Documento/SelecionarTags/{id}` - Mostra tags identificadas
  - `POST /Documento/SelecionarTags/{id}` - Salva seleção e reescreve
  - `GET /Documento/Resultado/{id}` - Exibe texto reescrito
  - `GET /Documento/Detalhes/{id}` - Histórico completo

### **4. Camada de Visualização (Views/Documento/)**

- **Index.cshtml**: Lista de documentos com cards
- **Novo.cshtml**: Formulário de entrada de texto
- **SelecionarTags.cshtml**: Interface para selecionar/modificar tags
- **Resultado.cshtml**: Comparação antes/depois + download
- **Detalhes.cshtml**: Histórico completo de processamentos
- **Processando.cshtml**: Tela de loading com auto-refresh

## 🔄 Fluxo Completo do Sistema

```
┌─────────────────────────────────────────────────────────────────┐
│ 1. USUÁRIO ENVIA TEXTO                                          │
│    - Acessa /Documento/Novo                                     │
│    - Cola texto (mínimo 50 caracteres)                          │
│    - Opcionalmente adiciona título                              │
└───────────────────┬─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────────────────────────┐
│ 2. SISTEMA CRIA DOCUMENTO                                       │
│    - Salva no banco (Status: Processando)                       │
│    - Cria Processamento tipo "IdentificacaoTags"                │
└───────────────────┬─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────────────────────────┐
│ 3. IA IDENTIFICA TAGS                                           │
│    - SemanticKernelService.IdentificarTagsAsync()               │
│    - Envia prompt estruturado para GPT-4o-mini                  │
│    - IA retorna JSON com tags + score de confiança              │
│    - Exemplo: {"codigo": "lista", "confianca": 0.95}            │
└───────────────────┬─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────────────────────────┐
│ 4. SISTEMA PERSISTE TAGS IDENTIFICADAS                          │
│    - Salva em ProcessamentoTagIdentificada                      │
│    - Registra PromptExecucao (rastreabilidade)                  │
│    - Redireciona para /Documento/SelecionarTags                 │
└───────────────────┬─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────────────────────────┐
│ 5. USUÁRIO SELECIONA/MODIFICA TAGS                              │
│    - Vê tags identificadas (pré-selecionadas)                   │
│    - Pode REMOVER tags identificadas                            │
│    - Pode ADICIONAR novas tags do catálogo                      │
│    - Submete seleção final                                      │
└───────────────────┬─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────────────────────────┐
│ 6. SISTEMA SALVA SELEÇÃO                                        │
│    - Salva em ProcessamentoTagSelecionada                       │
│    - Marca ação: Mantida / Adicionada / Removida                │
│    - Cria novo Processamento tipo "GeracaoConteudo"             │
└───────────────────┬─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────────────────────────┐
│ 7. IA REESCREVE TEXTO                                           │
│    - SemanticKernelService.ReescreverTextoAsync()               │
│    - Envia texto original + tags selecionadas                   │
│    - GPT-4o reescreve aplicando os formatos                     │
│    - Retorna texto transformado                                 │
└───────────────────┬─────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────────────────────────┐
│ 8. SISTEMA EXIBE RESULTADO                                      │
│    - Mostra comparação antes/depois                             │
│    - Permite copiar ou baixar texto                             │
│    - Opção de reprocessar                                       │
│    - Documento marcado como "Concluído"                         │
└─────────────────────────────────────────────────────────────────┘
```

## 📦 Configuração Necessária

### **1. Instalar Pacote NuGet**
```bash
dotnet add package Microsoft.SemanticKernel --version 1.38.0
```

### **2. Configurar appsettings.Development.json**
```json
{
  "OpenAI": {
    "ApiKey": "sua-chave-aqui",
    "ModeloIdentificacao": "gpt-4o-mini",
    "ModeloReescrita": "gpt-4o"
  }
}
```

### **3. Registrar Serviços em Program.cs**
✅ Já implementado:
```csharp
builder.Services.AddScoped<ISemanticKernelService, SemanticKernelService>();
builder.Services.AddScoped<IContentProcessingService, ContentProcessingService>();
```

### **4. Seed de Tags**
✅ 25 tags pré-configuradas (dissertação, lista, FAQ, tabela, etc.)

## 🎯 Por que NÃO usamos outros frameworks?

| Framework | Por que NÃO usar |
|-----------|------------------|
| **AutoGen** | Para múltiplos agentes conversando entre si. Overkill para seu caso. |
| **ML.NET** | Para treinar seus próprios modelos ML. Você já tem OpenAI. |
| **MAF** (Microsoft Agent Framework) | Ainda em preview, menos maduro que Semantic Kernel. |

## 💡 Alterações e Adições Feitas

### **Novas Classes Criadas:**
1. ✅ `TagIdentificacaoResponse` - Modelo de resposta da IA (identificação)
2. ✅ `TextoReescritoResponse` - Modelo de resposta da IA (reescrita)
3. ✅ `SemanticKernelService` - Serviço principal de comunicação com OpenAI
4. ✅ `ContentProcessingService` - Orquestrador do fluxo completo
5. ✅ `DocumentoController` - Controller com todas as rotas
6. ✅ `TagSeeder` - Seed de 25 tags pré-configuradas

### **Views Criadas:**
1. ✅ Index.cshtml - Listagem de documentos
2. ✅ Novo.cshtml - Formulário de entrada
3. ✅ SelecionarTags.cshtml - Seleção/modificação de tags
4. ✅ Resultado.cshtml - Exibição do resultado
5. ✅ Detalhes.cshtml - Histórico completo
6. ✅ Processando.cshtml - Loading state

### **Configurações Atualizadas:**
1. ✅ `appsettings.json` - Adicionada seção OpenAI
2. ✅ `Program.cs` - Registrados serviços de IA

## 🚀 Como Executar

1. **Adicione sua chave OpenAI**:
   ```json
   "OpenAI": {
     "ApiKey": "sk-..."
   }
   ```

2. **Execute migrações** (se necessário):
   ```bash
   dotnet ef database update
   ```

3. **Execute o projeto**:
   ```bash
   dotnet run
   ```

4. **Acesse**:
   ```
   https://localhost:5001/Documento
   ```

## 📊 Tabelas do Banco de Dados

```
Documentos
├── Processamentos (1:N)
    ├── ProcessamentoTagIdentificada (N:M com Tags)
    ├── ProcessamentoTagSelecionada (N:M com Tags)
    └── PromptExecucoes (1:N)
        └── PromptTemplate (N:1)
```

## ⚡ Vantagens da Arquitetura

1. ✅ **Rastreabilidade**: Toda interação com IA é registrada
2. ✅ **Versionamento**: Histórico completo de processamentos
3. ✅ **Flexibilidade**: Fácil adicionar novos tipos de processamento
4. ✅ **Auditoria**: Sabe exatamente o que foi enviado/recebido da IA
5. ✅ **Otimização**: Usa modelo mais barato (mini) para identificação

## 🔒 Segurança

- ✅ Autenticação obrigatória (Identity)
- ✅ Validação de propriedade de documentos
- ✅ Chave API não exposta no código
- ✅ Proteção CSRF em formulários

---

**Pronto para usar!** 🎉
