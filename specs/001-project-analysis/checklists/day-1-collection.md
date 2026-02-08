# Day 1 — Coleta automatizada e baseline

Propósito: coletar artefatos e métricas iniciais que servirão de base para a auditoria (AuditReport). Salve os resultados em `specs/001-project-analysis/artifacts/`.

Observações de segurança
- NÃO comite artefatos que contenham segredos (tokens, chaves privadas, dumps sensíveis). Exporte ou armazene esses artefatos em um local seguro e registre no relatório apenas a existência do achado e o local/commit onde foi detectado.

Pré-requisitos (recomendados)
- git (obrigatório)
- dotnet SDK (recomendado para projetos .NET)
- gh (GitHub CLI) ou GITHUB_TOKEN se quiser coletar histórico do Actions
- gitleaks ou trufflehog (opcional para detecção de segredos)
- reportgenerator (opcional para gerar relatório de cobertura)
- jq (opcional, para processar JSON)

Como usar (rápido)
1. Torne o script executável (opcional):

```bash
chmod +x specs/001-project-analysis/scripts/day1-collect.sh
```

2. Execute o script (executa coleções automáticas onde for possível):

```bash
bash specs/001-project-analysis/scripts/day1-collect.sh
```

3. Verifique artefatos gerados em `specs/001-project-analysis/artifacts/` e mova qualquer arquivo sensível para local seguro antes de commitar.

Checklist (tarefas)
- [ ] Criar pasta de artefatos: `specs/001-project-analysis/artifacts/`
- [ ] Coletar histórico CI (últimas 30 execuções) — salvar como `ci-runs.json`
- [ ] Executar `dotnet list package --vulnerable` e salvar saídas (`dotnet-vulnerable.txt`, `dotnet-packages.txt`)
- [ ] Rodar verificação de segredos (gitleaks/trufflehog) ou heurística de commits — salvar `gitleaks.json` ou `heuristic-secret-log.txt`
- [ ] Executar testes com coleta de cobertura (`dotnet test --collect:"XPlat Code Coverage"`) e salvar resultados (arquivos `*.trx` e `coverage`)
- [ ] Gerar relatório de cobertura (opcional) em HTML na pasta de artefatos
- [ ] Resumo: gerar `summary.txt` com lista de arquivos e comandos executados
- [ ] Popular `specs/001-project-analysis/findings.csv` com descobertas iniciais (manual ou via script de transformação)

Comandos úteis (manuais)

- Inferir owner/repo a partir do remoto GitHub:

```bash
REMOTE_URL=$(git config --get remote.origin.url || true)
GH_REPO=$(echo "$REMOTE_URL" | sed -E 's#.*github.com[:/]+([^/]+/[^/]+)(\.git)?#\1#')
echo "Detected repo: $GH_REPO"
```

- Coletar histórico do GitHub Actions (usando gh):

```bash
gh api "repos/${GH_REPO}/actions/runs?per_page=30" > specs/001-project-analysis/artifacts/ci-runs.json
```

- Coletar histórico do GitHub Actions (fallback usando curl + token):

```bash
export GITHUB_TOKEN="<token>"
curl -sS -H "Authorization: token $GITHUB_TOKEN" "https://api.github.com/repos/${GH_REPO}/actions/runs?per_page=30" \
  > specs/001-project-analysis/artifacts/ci-runs.json
```

- Dependências / vulnerabilidades (.NET):

```bash
dotnet list package --vulnerable > specs/001-project-analysis/artifacts/dotnet-vulnerable.txt || true
dotnet list package > specs/001-project-analysis/artifacts/dotnet-packages.txt || true
```

- Varredura de segredos (gitleaks):

```bash
```

- Heurística rápida (fallback) — procura por termos comuns em mensagens de commit:

```bash
git log --all --pretty=format:%B | egrep -i --line-number "password|passwd|secret|api[_-]?key|token|aws_access_key_id|aws_secret_access_key" \
  > specs/001-project-analysis/artifacts/heuristic-secret-log.txt || true
```

- Testes + cobertura (.NET com XPlat Code Coverage):

```bash
# tenta localizar solução
SLN=$(ls *.sln* 2>/dev/null | head -n1 || true)
if [ -n "$SLN" ]; then
  dotnet test "$SLN" --collect:"XPlat Code Coverage" --logger "trx;LogFileName=day1-results.trx" -v minimal || true
fi
# copie arquivos relevantes manualmente para artifacts/
```

- Gerar relatório de cobertura (reportgenerator, opcional):

```bash
```

Onde salvar/como nomear artefatos
- `specs/001-project-analysis/artifacts/ci-runs.json` — histórico CI
- `specs/001-project-analysis/artifacts/dotnet-vulnerable.txt` — saída dotnet vulnerable
- `specs/001-project-analysis/artifacts/dotnet-packages.txt` — lista de pacotes
- `specs/001-project-analysis/artifacts/gitleaks.json` ou `heuristic-secret-log.txt` — segredos detectados (manter fora do repositório público)
- `specs/001-project-analysis/artifacts/*.trx`, `coverage*.xml`, `coverage-report/` — resultados de teste e cobertura
- `specs/001-project-analysis/artifacts/summary.txt` — resumo de comandos e saída

Pós-coleta (sugerido)
- Revise os artefatos e preencha `specs/001-project-analysis/findings.csv` com descobertas relevantes.
- Crie as issues/tarefas para os top findings (ou use `specs/001-project-analysis/top-5-tasks.md` como template).

Problemas comuns e soluções rápidas
- Sem acesso ao Actions API: peça `GITHUB_TOKEN` com escopo `repo`/`repo:status` ou solicite ao responsável os logs de CI.
- `dotnet list package --vulnerable` não disponível: use scanners externos (OWASP Dependency-Check) ou verifique o painel de segurança do repositório.
- Ferramentas de detecção de segredos produzem falsos positivos: trate resultados como indícios — sempre confirme manualmente antes de tomar ação.

Se quiser, posso (A) gerar um script adicional para transformar os resultados JSON em linhas no `findings.csv`, ou (B) rodar as coleções automaticamente aqui e preencher `findings.csv` (precisará confirmar autorização para rodar ferramentas). Diga o que prefere.
