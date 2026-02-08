# Tasks: Auditoria do Projeto (001-project-analysis)

**Input**: Design documents from `/specs/001-project-analysis/` (plan.md, spec.md)

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Prepare artifact locations and run the initial automated collection so the audit has data to analyze.

- [ ] T001 [P] Create artifacts directory `specs/001-project-analysis/artifacts/` (add a `.gitkeep` file if needed)
- [ ] T002 Run the Day-1 collection script `specs/001-project-analysis/scripts/day1-collect.sh` and save outputs into `specs/001-project-analysis/artifacts/`
- [ ] T003 [P] Record installed CLI/tools availability and any missing tools by appending results to `specs/001-project-analysis/checklists/day-1-collection.md`

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Transform raw artifacts into structured findings and baseline metrics that the audit will use.

- [ ] T004 [P] Create a transformation script `specs/001-project-analysis/scripts/transform-artifacts-to-findings.sh` that extracts candidate findings from artifacts (`ci-runs.json`, `dotnet-vulnerable.txt`, `gitleaks.json`, `heuristic-secret-log.txt`) and appends rows to `specs/001-project-analysis/findings.csv`
- [ ] T005 Run the transformation script `specs/001-project-analysis/scripts/transform-artifacts-to-findings.sh` to populate or update `specs/001-project-analysis/findings.csv`
- [ ] T006 [P] Generate a CI summary file `specs/001-project-analysis/artifacts/ci-summary.md` by aggregating `specs/001-project-analysis/artifacts/ci-runs.json` (status counts, recent failures)
- [ ] T007 [P] Update `specs/001-project-analysis/audit-report.md` Baseline Metrics section with a placeholder table referencing collected artifacts (coverage, CI success rate, findings count)

## Phase 3: User Story 1 - Auditoria e relatório priorizado (Priority: P1) 🎯 MVP

**Goal**: Produce the AuditReport with findings by category, severity and at least one remediation per High/Critical item.

**Independent Test**: Follow `specs/001-project-analysis/acceptance/us1-acceptance.md` (created below) — after tasks complete it must show the AuditReport contains sections: Executive Summary, Baseline Metrics, Findings Summary, Findings (detalhado), Top-5 Improvement Tasks, Roadmap.

### Tests for User Story 1

- [ ] T008 [US1] Create acceptance test checklist `specs/001-project-analysis/acceptance/us1-acceptance.md` that asserts required sections and ACs exist in `specs/001-project-analysis/audit-report.md`

### Implementation for User Story 1

- [ ] T009 [US1] Parse artifacts (or use transform script results) and add at least one detailed finding row to `specs/001-project-analysis/findings.csv` (include id, category, severity, short_description, steps_to_reproduce, impact, recommendation, effort_estimate, owner_suggestion)
- [ ] T010 [US1] Add the detailed finding content in `specs/001-project-analysis/audit-report.md` under "Findings (detalhado)" for the row added in `findings.csv`
- [ ] T011 [US1] Populate `specs/001-project-analysis/top-5-tasks.md` with at least five improvement task entries derived from findings and include acceptance criteria and owner suggestions
- [ ] T012 [US1] Create verification instructions `specs/001-project-analysis/verification/us1-verification.md` that document how to validate the AuditReport against AC-FR-001..AC-FR-004

**Checkpoint**: AuditReport exists, `findings.csv` contains findings, `top-5-tasks.md` contains prioritized tasks. This is the MVP deliverable.

## Phase 4: User Story 2 - Plano de melhorias e quick wins (Priority: P2)

**Goal**: Convert audit findings into an executable roadmap and issue templates so the team can act on quick wins and medium/long work.

**Independent Test**: Review `specs/001-project-analysis/roadmap.md` and `specs/001-project-analysis/templates/issue-template.md` — each top-5 task must have a corresponding roadmap entry and an issue template.

- [ ] T013 [US2] Create `specs/001-project-analysis/roadmap.md` with Quick wins, Medium, Long items populated from `specs/001-project-analysis/top-5-tasks.md` (include impact and effort estimate)
- [ ] T014 [US2] Create issue template `specs/001-project-analysis/templates/issue-template.md` to convert a finding row into a tracker issue (title, description, acceptance criteria, estimate, owner suggestion)
- [ ] T015 [US2] [P] For each top-5 task, create a task file under `specs/001-project-analysis/tasks/` (e.g., `specs/001-project-analysis/tasks/task-001.md` .. `task-005.md`) that includes title, ACs, estimate and suggested owner

**Checkpoint**: Roadmap and task files exist and can be used to create tracker issues.

## Phase 5: User Story 3 - Melhoria da documentação e onboarding (Priority: P3)

**Goal**: Ensure a new developer can reproduce the build and run key checks within 60 minutes using the provided docs and runbooks.

**Independent Test**: A reviewer following `specs/001-project-analysis/quickstart.md` completes the Day-1 collection and runs the acceptance checklist in under 60 minutes.

- [ ] T016 [US3] Create `specs/001-project-analysis/quickstart.md` with step-by-step instructions to run `specs/001-project-analysis/scripts/day1-collect.sh`, run transform script, and open `specs/001-project-analysis/audit-report.md`
- [ ] T017 [US3] [P] Create onboarding checklist `specs/001-project-analysis/checklists/onboarding-checklist.md` that a new engineer can follow to verify they can run build+tests and produce baseline metrics

## Final Phase: Polish & Cross-Cutting Concerns

**Purpose**: Clean up, automation, and final verification across all stories.

- [ ] T018 [P] Update `specs/001-project-analysis/checklists/requirements.md` to mark status, include date and links to generated artifacts (`audit-report.md`, `findings.csv`, `top-5-tasks.md`, `roadmap.md`)
- [ ] T019 [P] Create a simple report generator script `specs/001-project-analysis/scripts/generate-report.sh` that consumes `findings.csv` and updates `specs/001-project-analysis/audit-report.md` templated sections (best-effort stub)
- [ ] T020 [P] Add `specs/001-project-analysis/README.md` linking to artifacts and describing next steps (how to convert tasks into tracker issues)

## Dependencies & Execution Order

- Phase 1 (Setup) must start first; T001 should run before T002. Many setup tasks are parallelizable and marked [P].
- Phase 2 (Foundational) depends on Phase 1 completion; T005 depends on T004 and T002 outputs.
- Phase 3 (User Story 1 - MVP) depends on Phase 2 completion (findings and baseline available). Implement US1 tasks before US2/US3 where possible.
- Phase 4 and Phase 5 can proceed after Phase 2; they are independent of each other but may use outputs from US1.

### Story completion order (recommended)
1. Foundation (T004..T007) complete
2. US1 (T008..T012) — MVP
3. US2 (T013..T015)
4. US3 (T016..T017)
5. Polish (T018..T020)

## Parallel execution examples

- While Foundation is running, run T004 (create transform script) and T006 (generate CI summary) in parallel: both are marked [P] and touch different files.
- Within US2, T014 (create issue template) and T013 (roadmap) can run in parallel [P].
- Create the five task files in T015 in parallel (`specs/001-project-analysis/tasks/task-001.md` .. `task-005.md`) — the task is marked [P].

## Implementation strategy

1. MVP first: Complete Phase 1 + Phase 2 + Phase 3 (User Story 1). Deliver AuditReport and top-5 tasks and validate with `specs/001-project-analysis/acceptance/us1-acceptance.md`.
2. Incremental: After US1 is validated, create roadmap and task files (US2), then onboarding docs (US3).
3. Automation: Implement T019 to enable reproducible generation of report contents from `findings.csv`.

## Format validation

- Total tasks listed: 20
- Tasks per story/phase:
  - Setup (Phase 1): 3 tasks (T001..T003)
  - Foundational (Phase 2): 4 tasks (T004..T007)
  - User Story 1 (US1): 5 tasks (T008..T012)
  - User Story 2 (US2): 3 tasks (T013..T015)
  - User Story 3 (US3): 2 tasks (T016..T017)
  - Final/Polish: 3 tasks (T018..T020)

- Parallel opportunities (marked [P]): T001, T003, T004, T006, T007, T015, T017, T018, T019, T020

- Independent test criteria per story:
  - US1: `specs/001-project-analysis/acceptance/us1-acceptance.md` must pass: AuditReport contains Executive Summary, Baseline Metrics, Findings Summary, Findings (detalhado), Top-5 Improvement Tasks and Roadmap; `findings.csv` has at least one finding.
  - US2: `specs/001-project-analysis/roadmap.md` contains Quick/Mid/Long items mapped to top-5 tasks; each top-5 task has a corresponding issue template.
  - US3: `specs/001-project-analysis/quickstart.md` and `specs/001-project-analysis/checklists/onboarding-checklist.md` allow a new engineer to reproduce baseline collection and run checks in ≤60 minutes.

- Suggested MVP scope: complete Phase 1 + Phase 2 + Phase 3 (User Story 1). This yields an executable AuditReport and top-5 tasks.

All tasks above follow the required checklist format: each line starts with `- [ ]`, includes a sequential Task ID (`T001`..`T020`), includes `[P]` for parallelizable tasks where applicable, includes story labels `[US1]`/`[US2]`/`[US3]` for user-story-specific tasks, and contains exact file paths where files are created or modified.
