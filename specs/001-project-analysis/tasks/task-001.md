# Task: Remediar vulnerabilidade em Microsoft.SemanticKernel.Core (F001)

Path: `specs/001-project-analysis/tasks/task-001.md`

Task ID: T001-REM-SEMANTICKERNEL (maps to finding F001)

Summary
- Objetivo: Remover ou atualizar a dependência vulnerável `Microsoft.SemanticKernel.Core` (1.68.0) identificada em `Saas.Web/Saas.Web.csproj` e validar que a correção não introduz regressões.

Acceptance Criteria
- AC-1: A versão vulnerável `1.68.0` não aparece mais em `dotnet list package --vulnerable` output for the solution.
- AC-2: Todas as builds CI principais passam (local check: `dotnet build` and `dotnet test` succeed for the solution).
- AC-3: PR criado com título claro, changelog/upgrade notes e referência para `specs/001-project-analysis/findings.csv#F001` and `specs/001-project-analysis/tasks/task-001.md`.

Steps
1. Create branch `fix/deps/F001-semantic-kernel` from the current feature branch.
2. Update `Saas.Web/Saas.Web.csproj` to a non-vulnerable version of `Microsoft.SemanticKernel.Core` (or follow vendor guidance). If no fixed version exists, implement compensating control and document it.
3. Run `dotnet restore && dotnet build` and `dotnet test` locally; capture results in `specs/001-project-analysis/artifacts/` (do not commit secrets or large logs).
4. Run `dotnet list package --vulnerable` and confirm no critical vulnerabilities remain for the updated package.
5. Update this task file with PR link and final verification notes.

Estimate: 1d
Owner suggestion: Backend owner / maintainer
