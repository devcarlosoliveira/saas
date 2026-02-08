# AuditReport — Análise do Projeto (001-project-analysis)

Feature: `001-project-analysis`
Created: 2026-02-08
Author: [Lead Auditor]
Status: Draft

## Executive Summary

[Resumo curto do estado do projeto, principais riscos e recomendações de alto impacto.]

## Delivery Model

Audit model: audit-only — este documento não contém alterações de código; remediações serão criadas como tarefas separadas.

## Compliance

[Indicar requisitos legais/compliance identificados ou declarar explicitamente "Nenhum requisito de conformidade especial identificado". Se dados sensíveis forem encontrados, descreva como foram reportados e escalonados sem expor valores.]

## Baseline Metrics

Incluir uma tabela com métricas coletadas (ex.: cobertura de testes por módulo, taxa de sucesso do CI nas últimas 30 execuções, contagem de findings por severidade). Referência: `findings.csv`.

## Findings Summary

Listagem agregada das descobertas por categoria e severidade. Para detalhes exportados, consulte `findings.csv`.

## Findings (detalhado)

Para cada finding incluir:
- ID
- Categoria
- Severidade
- Descrição curta
- Passos para reproduzir / evidências
- Impacto
- Recomendação (mitigação rápida e solução completa)
- Estimativa de esforço
- Proprietário sugerido

## Top-5 Improvement Tasks

Resumo das cinco tarefas de maior impacto. Detalhes e critérios estão em `top-5-tasks.md`.

## Roadmap Prioritário

Tabela com itens categorizados (Quick wins, Médio prazo, Longo prazo), impacto e esforço estimado.

## Appendices / Artefatos

- `findings.csv` — tabela com todas as descobertas
- Logs / extratos do CI (anexar ou referenciar local seguro)
- Resultados de scanners (dependências, secret-scan, linters)
- Comandos e passos executados para reproduzir a análise

## How to use this document

1. Leia o Executive Summary para entender os riscos mais críticos.
2. Use a seção Findings para localizar detalhes e evidências.
3. Verifique `top-5-tasks.md` para as ações imediatas recomendadas.
4. Crie issues com base nas tasks priorizadas ou agende sessão de priorização com o PO.
