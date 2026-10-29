---
name: solar-sandbox-implementation
description: Implement or test a scoped Solar Sandbox sprint feature in a Blazor WebAssembly solar system editor, electrical topology validator, or deterministic energy simulation engine. Use for coding, refactoring, debugging, and verifying MVP requirements and project portability.
---

# Implement a Solar Sandbox feature

1. Read `MASTER_SPEC.md`, identify requirement IDs and statuses. Read `SKILLS.md` for the implementation playbook. If a required behavior is OPEN, explain the blocker rather than silently inventing an engineering decision.
2. Define a small vertical slice with a demo scenario and acceptance test. Keep the private beta usable at every milestone.
3. Model graph nodes and typed ports separately from their canvas coordinates and rendered wire paths.
4. Keep the simulation engine deterministic in pure C#. Use energy in Wh, time in hours, and separate AC/DC and demand/served load. Enforce battery bounds, allowed operating modes, and explicit unsupported states.
5. Implement validations as typed diagnostics with IDs, severities, traceable assumptions, and explicit UNKNOWN cases. Never certify physical electrical safety.
6. Add automated tests and a clear manual offline reproduction path. Preserve JSON versioning, autosave integrity, and tablet interactions where touched.
7. Run the relevant build/tests; include requirement IDs, test evidence, limitations, and docs updates in the implementation summary.

Use the existing repository architecture. Do not add a backend, external paid services, unapproved frameworks, extra brand catalogs, or community features without an approved spec change.
