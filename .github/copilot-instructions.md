# Solar Sandbox — Repository Guidance for GitHub Copilot

The single authoritative product specification is `MASTER_SPEC.md`. Read the relevant W.O.U.L.D. sections and requirement IDs before proposing or changing code. The companion `SKILLS.md` is a development playbook; it is not an alternate source of product requirements.

## General Guidelines
- Current goal: **working private beta**. Prefer vertical slices with tests and an executable Demo System over broad feature work.
- Use Blazor WebAssembly and a pure, testable C# domain/simulation core as the current proposed direction, subject to open architecture decisions in the master.
- Preserve 100% offline core functionality, local autosave, portable versioned JSON, and touch-friendly canvas interactions.
- Keep UI appearance distinct from electrical topology; compute only from valid connected components.
- Never invent vendor specifications, electrical code compliance, neutral/ground safety conclusions, battery limits, or precise scenario failure times. Missing information must be explicitly marked UNKNOWN/NOT MODELED.
- Maintain unit-correct energy calculations and deterministic, tested simulation runs.
- For each change: state requirement IDs, acceptance criteria, tests, assumptions, and any open decision. Stop and ask the owner for product decisions that the master marks OPEN.
- No community features, cloud accounts, or backend until explicitly approved.

## UI/UX Guidelines
- Design a smooth, comforting, DIY-friendly UI; avoid a rigid CAD-like appearance.
- Implement a sidebar layout instead of a header bar.
- Use Lucide icons for UI icons.
- Include a working dark/light mode toggle by default.

## Blazor Best Practices
- Follow Blazor best practices for components and layout patterns.
- Use JavaScript in separate files only; avoid inline root HTML scripts.
- Implement event delegation for JavaScript button handling.
- Utilize MudBlazor as the UI framework.
- Include automatic light/dark mode by default.
