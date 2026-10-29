# Sprint 0 — Gate 0 Decision Closure

This document captures owner-approved Gate 0 decisions required before Gate 1 implementation.

## Decision Baseline (Approved)

1. Canonical project store: **IndexedDB only**
2. Invalid wiring policy: **prevent invalid edge creation**
3. Default simulation step: **60 minutes**
4. Gate 1 weather mode: **manual preset only**
5. Starter equipment set for Gate 1: **generic only**
6. PV array stringing details in Gate 1: **deferred**
7. Diagnostic severities: **Info / Warning / Critical**
8. Unknown equipment specs: **simulate known values + UNKNOWN flags**
9. Import newer schema: **reject with explanation**
10. Launch behavior: **auto-open last active project**
11. Autosave behavior: **debounced edits + lifecycle flush**
12. First-run project options: **Blank Canvas + Demo System**
13. Gate 1 export minimum: **project JSON export only**
14. Disconnected loads: **not counted as energized load**
15. Milestone quality gate: **unit + integration tests required**

## Acceptance Criteria for Gate 0 Closure

- All decisions above are documented and referenced by Sprint 1 implementation tasks.
- No Gate 1 implementation introduces behavior that conflicts with this baseline.
- Any unresolved OPEN behavior from `MASTER_SPEC.md` remains explicitly marked as deferred or unknown.
- Sprint artifacts support small feature-block branches and reviewable commits.

## Implementation Backlog Seed (Gate 1 Ready)

1. Create startup flow for first run and last-project resume.
2. Define IndexedDB-backed repository interface and minimal project schema envelope.
3. Implement blank project creation and demo project creation path.
4. Implement typed-port edge creation guard that blocks invalid edges.
5. Add base diagnostics model with Info/Warning/Critical severities.
6. Implement deterministic simulation scaffold with 60-minute default step.
7. Implement manual weather preset input model (no external weather integration).
8. Ensure disconnected loads are excluded from energized-load computation.
9. Add project JSON export endpoint/action for current project.
10. Add tests (unit + integration) for the above vertical slice.

## Assumptions / Explicit Non-Goals in Sprint 0

- No cloud backend or account system.
- No non-generic branded equipment in Gate 1.
- No PV stringing detail implementation in Gate 1.
- No silent inferred electrical safety claims.

## Traceability Notes

Primary requirement source remains `.github/MASTER_SPEC.md`.
Implementation rules remain `.github/copilot-instructions.md`.
