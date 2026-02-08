<!--
Sync Impact Report
Version change: none → 1.0.0
Modified principles:
- Added: I. Test-First (NON-NEGOTIABLE)
- Added: II. Reproducible Local Development
- Added: III. Continuous Integration & Gatekeepers
- Added: IV. Observability & Structured Logging
- Added: V. Semantic Versioning & Breaking Changes
Added sections:
- Operational Constraints & Security Requirements
- Development Workflow & Quality Gates
Removed sections:
- None
Templates requiring updates:
- ✅ .specify/memory/constitution.md (updated)
- ✅ .specify/templates/plan-template.md (updated)
- ✅ .specify/templates/spec-template.md (updated)
- ✅ .specify/templates/tasks-template.md (updated)
- ✅ .specify/templates/agent-file-template.md (updated)
- ✅ README.md (updated)
- ⚠ .specify/scripts/bash/update-agent-context.sh (manual review recommended for agent-specific references)
- ⚠ .specify/templates/checklist-template.md (review for optional items)
Follow-up TODOs:
- None
-->

# Saas.Web Constitution

## Core Principles

### I. Test-First (NON-NEGOTIABLE)
All feature work and production code changes MUST be accompanied by automated tests written before implementation. Tests
MUST fail initially, express the intended behavior unambiguously, and demonstrate expected outcomes including edge cases
and error paths. The repository enforces the Red‑Green‑Refactor cycle: write failing tests → implement minimal code to pass →
refactor. Pull requests that add or change behavior MUST include tests exercising the new behavior. CI MUST run the test
suite and block merges on failure.
Rationale: Test‑first development prevents regressions, enforces clear acceptance criteria, and makes intent explicit.

### II. Reproducible Local Development
Developers MUST be able to reproduce the local development environment and run the full test suite using documented
commands. Required SDKs and tools (for example: .NET 10, `dotnet-ef`) MUST be documented in the repository README or
tool manifests. Repository-provided setup scripts MUST be idempotent and version-controlled.
Rationale: Reduces onboarding time and prevents environment-driven bugs.

### III. Continuous Integration & Gatekeepers
All changes MUST be made via pull requests. Every PR MUST pass CI that includes: dependency restore, build, lint/format
checks, and the automated tests. Database schema changes MUST include a migration artifact and a short migration plan
describing upgrade and rollback steps. Merges that change public contracts MUST include a compatibility and migration plan
and require at least two reviewer approvals.
Rationale: Protects mainline stability and makes breaking changes explicit and auditable.

### IV. Observability & Structured Logging
The application MUST emit structured logs for operational and error events, carry a correlation ID for request flows, and
expose meaningful metrics for key business paths. Error conditions MUST include enough context to diagnose root cause.
Plans that add or modify critical flows MUST document how those flows are covered by logs and metrics.
Rationale: Observability is required to operate, diagnose, and improve the service reliably.

### V. Semantic Versioning & Breaking Changes
Releases MUST follow semantic versioning (MAJOR.MINOR.PATCH). Breaking changes to public contracts or shared schemas MUST
increment the MAJOR version, be documented, and ship with migration plans or deprecation paths. Additive, compatible
behavior changes increment MINOR; bugfixes and non‑behavioral edits increment PATCH.
Rationale: Predictable versioning and clear upgrade paths reduce disruption for integrators.

## Operational Constraints & Security Requirements

- Technology stack: .NET 10 (primary), ASP.NET Core, Entity Framework Core. Development default DB: SQLite; production
  database choices and connection details MUST be documented per-deployment.
- Secrets MUST NOT be committed to source. All secrets MUST be injected via environment variables or a secret manager.
- Sensitive personal data (PII) MUST be handled according to applicable data protection policies; storage and retention
  MUST be explicit in the feature spec when PII is involved.
- Dependency updates and security scans MUST run in CI; critical vulnerabilities MUST be remediated on an accelerated timeline.

## Development Workflow & Quality Gates

- Feature work follows: `spec.md` → `plan.md` → `tasks.md` → PRs implementing tasks with tests.
- Every plan (`plan.md`) MUST include a "Constitution Check" listing how the plan satisfies each core principle.
- PRs MUST include a short checklist showing which constitutional requirements the change touches (tests added,
  migration included, observability updated, versioning impact).
- Code reviews MUST verify compliance with constitutional MUSTs and may require follow-up tasks for any gaps.

## Governance

- Amendments: Changes to this constitution MUST be proposed via a pull request modifying `.specify/memory/constitution.md`.
  The PR MUST include:
  1. A concise summary of the change and rationale.
  2. A list of affected templates, docs, or runtime artifacts and a migration plan where applicable.
  3. Tests or validation steps (where applicable) to show the change is realized.
- Approval: Constitutional changes MUST be approved by at least two maintainers (or equivalent governance group).
  Emergency amendments may be applied by a single maintainer but require documented retrospective review within 7 days.
- Versioning policy: Constitution versions follow semantic rules:
  - MAJOR: Backward‑incompatible redefinitions or principle removals.
  - MINOR: New principle or material expansion of guidance.
  - PATCH: Clarifications, wording fixes, and non‑semantic refinements.
- Compliance reviews: The project’s analysis tools MUST treat constitution violations as blocking findings. Plans and PRs that
  violate MUSTs are considered critical and require remediation before merge.

**Version**: 1.0.0 | **Ratified**: 2026-02-08 | **Last Amended**: 2026-02-08
