# Solar Sandbox Sprint Plan (Milestone-Based)

This plan is organized by feature blocks to keep pull requests small, reviewable, and demo-ready.

## References
- Authoritative spec: [MASTER_SPEC.md](./MASTER_SPEC.md)
- Repo implementation rules: [copilot-instructions.md](./copilot-instructions.md)

## Sprint 0 — Gate 0 (Decision Closure)

Goal: lock implementation decisions required to start coding without inventing OPEN behavior.

Feature blocks:
1. Persistence contract baseline (IndexedDB canonical store)
2. Wiring validity policy (prevent invalid edge creation)
3. Simulation baseline semantics (60-minute default step)
4. Diagnostics baseline (Info/Warning/Critical)
5. Unknown-spec behavior (simulate known values + UNKNOWN flags)
6. Import safety baseline (reject newer schema with explanation)
7. First-run and launch behavior (Blank Canvas/Demo System; auto-open last project)
8. Autosave baseline (debounced edits + lifecycle flush)
9. Test gate definition (unit + integration per milestone)

Deliverable: [SPRINT_0_GATE0.md](./SPRINT_0_GATE0.md)

## Sprint 1 — Gate 1 (Vertical Slice)

Goal: first deployable offline slice with one coherent end-to-end path.

Feature blocks:
1. Offline shell and startup flow
2. Blank Canvas + Demo System project bootstrap
3. Generic equipment starter set only
4. Minimal node/port connect flow with invalid-edge prevention
5. Manual weather preset only
6. Deterministic scenario run (default 60-minute step)
7. Project JSON export (minimum export scope)
8. Unit + integration test slice

## Sprint 2 — Gate 2 (Complete Circuit Foundations)

Feature blocks:
1. Expanded source/load graph behavior
2. Power path invariants and disconnected-load handling
3. Autosave hardening and restore reliability
4. Import validation hardening and schema migration guardrails

## Sprint 3 — Gate 3 (Scenario + Evidence)

Feature blocks:
1. Scenario evidence outputs and diagnostics traceability
2. Deterministic replay validation
3. Reporting foundation and acceptance fixtures

## Sprint 4 — Gate 4 (MVP-A Readiness)

Feature blocks:
1. Tablet usability and quality polish
2. Resilience and failure-mode handling
3. Private beta release checklist and smoke validation

## Branch / Commit Guidance

For healthy public history:
- One feature block per branch where practical
- Keep PRs narrow and test-backed
- Reference requirement IDs and sprint block in PR descriptions
- Avoid mixing refactors with feature behavior changes
