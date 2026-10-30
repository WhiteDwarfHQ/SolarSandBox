# SOLAR SANDBOX
## Master MVP Specification — W.O.U.L.D. Planning Document
**Version:** 0.2 — Living requirements baseline / review draft  
**Date:** October 29, 2026  
**Status:** W approved; O1–O3 approved; O4+, U, L and D under review  
**Audience:** Product owner, engineering contributors, AI coding agents, testers  
**Source:** Original Off-Grid Topology Tracer concept; subsequent product decisions and refinements.

## Implementation status snapshot (working branch progression)
- **F_001 complete:** startup shell, dark-first UI baseline, theme toggle, first-run project modal, last-project reopen.
- **F_002 complete:** project JSON envelope, demo/blank bootstrap, IndexedDB compatibility fixes, export action, baseline unit+bUnit tests.
- **F_003 in progress:** electrical domain foundations for equipment definitions/instances, typed electrical ports, independent connection validation service, schema v2 document extension, migration-safe persistence defaults, and minimal observable validation workflow (no simulation engine scope in this milestone).

> **North star:** A fast, beautiful, tablet-first, entirely usable offline solar design studio where DIY builders and residential contractors connect real or custom equipment, inspect potential electrical incompatibilities, and simulate the energy consequences of their designs before purchase or installation.

> **Safety boundary:** Solar Sandbox is a planning/education and estimation tool. Its diagrams, compatibility warnings, and simulations do not certify code compliance or replace equipment manuals, qualified design review, electrical permits, or field testing.

## Document control and decision taxonomy
- **CONFIRMED:** Explicitly approved during requirements rounds or subsequent refinements.
- **PROPOSED:** Working implementation recommendation, not yet authorized.
- **OPEN:** Requires owner choice, research, or a prototype before acceptance.
- **LATER:** Roadmap concept; not a launch requirement.
- **CONFLICT:** Two source requirements or examples cannot both be accepted without clarification.

This is a requirements baseline, not a claim that every technology and rule is final. Decisions should receive an ID, date, rationale, and revision history. The original file repeats parts of its specification; repetition has been consolidated without discarding unique behavior.

## Current approvals — owner-verified decisions (October 29, 2026)
- **W APPROVED:** Target users, initial walkthrough, unified approachable professional interface and offline-first experience. Private beta comes first; public launch is not a current deliverable.
- **O1 APPROVED — projects:** Reopen last active project by default; first-run or New Project offers **Blank Canvas** or one **Demo System**. The demo is a known-good generic/DIY system and an automated/manual regression fixture. Multi-diagram/multi-scenario projects, local autosave, portable JSON and destructive-action confirmation are in scope.
- **O2 APPROVED — canvas:** Effectively infinite pan/zoom, movable nodes, distinct grid and snap toggles, orthogonal auto-routed connections, guided valid-port highlighting, undo/redo and touch-first editing. **Grouping deferred.**
- **O3 APPROVED — equipment:** Small EG4 + generic + DIY starter library. Place/edit project-specific copies of catalog profiles; support individual batteries and simplified bank nodes, preset/custom scheduled loads, and integrated portable station representations. Unknown values are surfaced; illustrative estimates require explicit opt-in, not invisible assumptions.
- **O4 OPEN — electrical diagnostics:** Core intent agreed but detailed topology algorithms, bonding assumptions, power-flow models and validation severities are not yet accepted.
- **LATER / OUT OF MVP:** Community system submissions, cloud database, user accounts, shared gallery and syncing. Optional live-weather APIs remain an architectural extension, not a required core dependency.

## Scope discipline and decision gate
This living specification records what is decided and what is still proposed. **Do not interpret a proposed formula, manufacturer profile, architecture library, or code-validation rule as approved merely because it appears in the document.** The first shipping gate is a small testable private beta. `SKILLS.md` and sprint tickets will be created from the approved baseline after the outstanding technical rules are resolved.

# W — Walk Through It
## W1. Product definition
Solar Sandbox (working name; original name: Off-Grid Topology Tracer) is a browser-delivered, installed-capable Progressive Web App (PWA). The application offers an effectively limitless, navigable canvas for laying out generation, conversion, storage, distribution, protection, control, grid/generator sources, and electrical loads. A browser-local C# engine estimates energy generation and consumption over time and performs documented electrical topology checks. Projects can be edited offline and moved between devices as versioned portable files.

## W2. Target users and jobs to be done
- **DIY/off-grid builder (CONFIRMED):** Explore architectures, compare storage needs, learn tradeoffs, model unusual salvaged or homebuilt parts without pretending those are automatically safe.
- **Residential solar contractor (CONFIRMED):** Sketch residential-scale arrangements on a tablet, reason about compatibility, estimate endurance, and share legible printable results.
- **Homesteader/smart-home operator (CONFIRMED):** Include always-on idle consumption from Home Assistant hosts, network switches, PoE cameras, radio hubs, and timed outdoor loads.

## W3. Core walk-through — first successful session
1. Launch installed PWA with network disabled. On a first run, choose **Blank Canvas** or **Demo System**; on subsequent launches automatically restore the last active project with a prominent **New Project** action. The workspace, toolbox, telemetry and diagnostics load without an external request.
2. Add an array with 6 panels rated 400 W each; configure array topology (series/parallel stringing is an OPEN detail). At nameplate STC, 6 × 400 W = 2,400 W DC. A simple illustrative weather multiplier of 0.15 yields 360 W DC before system losses; do not label this a guaranteed actual output.
3. Add an EG4 inverter profile and optional GridBOSS equipment, then wire compatible typed ports. Each port identifies purpose, AC/DC domain, supported voltage/current ranges, and allowed connector types.
4. Add a **custom 201.6 V nominal salvaged battery** to a low-voltage battery input profile: the connection is marked incompatible and refused when the equipment input range excludes that voltage. Explain the incompatible **battery input** separately from the PV MPPT input. Do not imply any conversion device makes salvaged packs safe without engineering validation.
5. Replace it with a compatible nominal-voltage battery profile. Validate the **full operating voltage range**, current limits, chemistry/BMS limitations, permitted battery combinations, and model-specific compatibility assumptions; warn about any unknowns.
6. Add a distribution panel with LED lighting (120 W), always-on networking (15 W), and a 1,500 W tank heater scheduled overnight (18:00–06:00). Connect source-to-load paths, protection and neutral/ground metadata as supported by the selected detail mode.
7. Observe derived values for PV, AC consumption, battery power, battery energy and SOC. Disconnected equipment cannot contribute as if energized.
8. Start an Ice Storm / Grid Failure scenario, review plots and first failure hour, change battery quantity and re-run the same reproducible scenario.
9. Print a report and export a portable project file; import it into a different browser/device and verify the diagram and scenarios are preserved.

## W4. Canvas and product experience
- **CONFIRMED:** Free-form drag/drop nodes; connect lines by snapping ports; move nodes independently; visual power paths; dark professional studio UI; center canvas with collapsible tools, inspector, live telemetry, and diagnostics.
- **CONFIRMED:** Effectively infinite spatial canvas; pan/zoom, tablet pinch; visible grid and snap-to-grid toggles; no arbitrary practical diagram boundary.
- **CONFIRMED:** Mixed-size representations: compact switch/symbol nodes, normal equipment cards, richer branded/inverter cards, optional expanded properties.
- **CONFIRMED:** Multiple diagrams and multiple simulation scenarios per project.
- **PROPOSED:** Select/drag/undo/redo, fit-to-view, touch hit targets, mini-map, alignment aids, keyboard shortcuts on desktop, and a clear distinction between merely selected and actually connected ports.
- **LATER:** Group several nodes into collapsible reusable subsystems (e.g., Barn Power System). Community uploads, galleries, shared libraries and any account/database service are explicitly out of scope.

## W5. Outcomes and success measures
1. **Real utility:** A first-time user can build, save, simulate, and report a simple residential off-grid system without documentation-driven handholding.
2. **Offline integrity:** All design, simulation, catalogs, and local persistence functions work after offline installation; weather API is an optional additive source, never required to launch.
3. **Simulation credibility:** Energy accounting preserves units and obeys component and battery constraints; tests cover boundary cases and time progression.
4. **Trustworthy diagnostics:** No unqualified statement that a configuration is code-compliant or electrically safe. Unknown information is surfaced, not silently treated as valid.
5. **Tablet usability:** A contractor can manipulate and inspect a realistic diagram with touch without accidental wiring or lost edits.
6. **Delivery:** Ship a versioned **private beta** to the owner and a small invited tester group, with install directions, a working demo project, known limitations and repeatable end-to-end tests. Public release is a separate later decision. A shipped working slice beats a never-finished perfect vision.

# O — Open Up the Requirements
## O1. Confirmed product decisions
| ID | Requirement | Status |
|---|---|---|
| O-01 | Audience includes DIY users and residential contractors | CONFIRMED |
| O-02 | Free-form drag/drop node editor and snap-to-connect wiring | CONFIRMED |
| O-03 | MVP spans arrays, batteries, inverters, grid, panels, loads and simulation | CONFIRMED |
| O-04 | Local autosave and portable JSON import/export | CONFIRMED |
| O-05 | Built-in starter equipment catalog, EG4, generic and DIY custom nodes | CONFIRMED |
| O-06 | Core works entirely offline; no server dependency or mandatory account | CONFIRMED |
| O-07 | Functional connection editor with optional advanced electrical properties | CONFIRMED |
| O-08 | Hourly scenario energy estimates, weather effects, efficiency and load schedules | CONFIRMED |
| O-09 | Block incompatible physical/typed ports; warn on hazardous/questionable configurations | CONFIRMED |
| O-10 | Professional studio tablet interface and live diagnostics | CONFIRMED |
| O-11 | Multiple diagrams and scenarios per project | CONFIRMED |
| O-12 | Offline weather profiles, seasonal daylight settings, custom weather inputs | CONFIRMED |
| O-13 | Printable project/system reports and equipment/energy summaries | CONFIRMED |
| O-14 | Additional weather modes: live API, cached forecasts, synthetic/randomized conditions | CONFIRMED direction; MVP split OPEN |
| O-15 | Effectively infinite canvas; zoom/pan; optional visible grid and snap | CONFIRMED |
| O-16 | Equipment catalog covers common residential/DIY equipment, potentially portable/hybrid Jackery-style systems; avoid industrial completeness | CONFIRMED direction |
| O-17 | Different node icon/card visual treatments | CONFIRMED direction |

## O2. Functional requirements — organized by subsystem
### Project lifecycle (P)
- **P-01:** Create/open/rename/duplicate/delete project with explicit confirmation for destructive deletion. New Project offers **Blank Canvas** or exactly one **Demo System** at MVP start. On a first launch present these choices; on later launches reopen the last active project automatically (if present).
- **P-02:** Create/rename/duplicate/delete diagrams within a project; diagrams do not share node instances by accident.
- **P-03:** Autosave changes locally and recover from reload, navigation or transient browser closure when persistent storage remains available.
- **P-04:** Export/import a **versioned JSON** package containing diagrams, nodes, edges, custom profiles, scenarios and project metadata. Report unsupported versions without overwriting existing data.
- **P-05:** Track dirty/saved/error status and storage errors visibly; include manual export/backup instructions. Autosave is not a guarantee against cleared browser data.

### Diagram editor (C)
- **C-01:** Add equipment from toolbox to virtual canvas via drag/drop or accessible tap-to-add.
- **C-02:** Distinct typed input/output ports, connection gesture, endpoint and type feedback, connect/disconnect/delete behavior. While wiring, **highlight compatible destination ports** and guide users away from incompatible targets.
- **C-03:** Prevent connections that have no physically meaningful port mapping or violate known hard interface constraints. Preserve explanatory error details.
- **C-04:** Pan/zoom/fit and move nodes without changing their electrical meaning; grid visibility and snapping independently toggleable. Draw **clean automatically routed orthogonal wires with bends**, avoiding overlapping nodes when practical.
- **C-05:** Inspect/edit node profile, name, numeric parameters, limits, schedule and annotations; reflect changes in diagnostics.
- **C-06:** Show energized/disabled/fault/unknown or informational states without relying solely on wire color.
- **C-07:** Support diagram undo/redo or mark as launch blocker if omitted; user must not lose complex edits to single accidental gesture.

### Catalog and custom equipment (E)
- **E-01:** Built-in generic solar array, battery/bank, inverter, PV charge controller/MPPT, grid source, generator, transfer/interlock, distribution panel, switch/relay/timer, and generic load profiles as warranted by supported topology.
- **E-02:** Starter verified brand profiles centered on EG4; add portable all-in-one category where electrical port capabilities justify it. Actual model list and licensing/brand asset policy OPEN.
- **E-03:** User-defined equipment with name, domain, ports, voltage/current/power envelope, efficiencies, capacity, idling and documentation status. Editing catalog equipment creates a **project-specific profile copy**, never mutating the built-in catalog. Support both individual series/parallel batteries and simplified bank nodes.
- **E-04:** Distinguish user-entered specs, vendor-verified specs, and illustrative/sample values. Every branded profile needs product model/revision, source document/date and verified limitations.
- **E-05:** Do not infer a branded all-in-one supports arbitrary power sharing, stacking, export, split phase or bypass; capabilities are explicit per profile. Portable power stations are integrated models of storage, inverter and supported charging/output ports. Include preset and custom loads with schedules.

### Simulation and telemetry (S)
- **S-01:** Calculate nameplate array peak and modeled environmental output separately.
- **S-02:** Determine active connected loads and idle loads, including independent schedules and operating modes.
- **S-03:** Compare active AC apparent/real power and inverter continuous limits using explicit assumptions; warn for unknown power factor/surge.
- **S-04:** Show PV production, served load, unmet load, grid/gen import, battery charging/discharging, usable energy and SOC with units.
- **S-05:** Scenario engine supports time-steps over multiple days, variable daylight/weather, grid changes, generator state, scheduled loads and configurable initial SOC.
- **S-06:** Scenario outcome identifies first outage/unsupported load interval, energy balance, lowest SOC, and the actual computed time of cutoff. Never hard-code dramatic times or battery counts.
- **S-07:** Scenario reruns are deterministic with same inputs and random seed; retain the named scenario and output snapshot/version.
- **S-08:** Weather sources are pluggable: manual, preset, seasonal, randomized; live/cached network forecasts are optional modules and require explicit refresh, date/source/freshness labels, and consent considerations.

### Diagnostics and output (D/R)
- **D-01:** Severity model INFO / CAUTION / WARNING / CRITICAL / UNKNOWN, with specific node/edge references, why it matters, and remediation guidance without asserting certification.
- **D-02:** Detect known mismatches: AC vs DC, incorrect port type, voltage/current envelope exceedance, supply overload, disconnected inputs, unsupported topology, unavailable grid, and conflicting modeled bonding assumptions.
- **D-03:** Neutral-ground rule is **topology-/mode-dependent**; counting all bonded nodes is not sufficient. Until validated, report possible conflict/unknown rather than claiming definitive electrical code findings.
- **D-04:** Detect absence of required protective/disconnection devices where supported by model, but distinguish missing metadata from proven missing field hardware.
- **R-01:** Printable report: title/date/version, diagram, device inventory, stated assumptions, scenario settings/results, severe diagnostics, and safety disclaimer.
- **R-02:** Print uses browser-local rendering; PDF output via browser print-to-PDF may be offered without introducing a backend.

## O3. Deliberate scope boundary — ship the first usable product
**Minimum private-beta scenario:** One project containing more than one diagram; add PV, inverter, battery, grid, subpanel and loads; connect, edit, validate, autosave, import/export, run repeatable weather/grid-loss simulation, view SOC and diagnostics, and print a report, entirely offline.

**Private-beta candidate (MVP-A, PROPOSED):** One mature end-to-end editor and energy model; a small **verified** EG4/generic starter catalog; custom equipment; a handful of credible offline presets; schedules; deterministic results; stable storage and tablet workflow. No claim of NEC compliance.

**Incremental release (MVP-B, PROPOSED):** Add broader branded hardware, optional online forecast import/caching, richer wire/protection inspection, additional visual styles, and better charting after MVP-A performs reliably.

**Future releases (LATER):** Deep terminal-level conductor modeling, comprehensive national electrical code engine, automatic code signoff, automatic engineering BOM, real-time Home Assistant device integration, remote accounts/sync, collaborative editing, reusable hierarchical subsystems, industrial catalog coverage, live IoT control, cloud services. These are explicitly **not** part of initial acceptance unless the owner changes the scope.

## O4. Non-functional requirements
- **N-01 Performance:** Target responsive editing on a midrange Android tablet and desktop. Proposed acceptance: input feedback under 100 ms for typical local actions; a representative 75-node/100-edge diagram remains usable with smooth interaction; measure on target hardware before finalizing budgets.
- **N-02 Offline:** Installed app supports cold navigation to cached routes and full local execution after first successful asset caching; no CDN dependency for core assets, fonts, icons or catalog. Version upgrades must not silently strand user data.
- **N-03 Privacy:** No tracking, mandatory auth, mandatory cloud upload or opaque telemetry. Explain that optional live weather requests disclose location/query to a provider; never silently transmit full project content.
- **N-04 Reliability:** Validate numeric range/units, reject malformed imported files, protect current state on failure, show autosave error conditions.
- **N-05 Accessibility:** Touch-friendly targets, zoom controls, keyboard alternatives, descriptive port/diagnostic labels and color-independent error indicators.
- **N-06 Maintainability:** Separate visual presentation, domain graph, simulation rules, equipment catalog and persistence using interfaces, tests and clear versioning.
- **N-07 Portability:** Avoid browser-specific file APIs without tested fallbacks; identify service-worker and storage limitations.
- **N-08 Deployment:** Static-host compatible build; one documented production URL and a tested install/update flow.

## O5. Key ambiguities and decisions still open
| ID | Open decision | Proposed default |
|---|---|---|
| Q-01 | Blazor-only vs specialized JS canvas interactions | Blazor WASM + minimal JS interop if validated |
| Q-02 | Diagram library vs custom canvas | Spike established library on Android tablet first |
| Q-03 | IndexedDB vs alternate storage | IndexedDB with application repository abstraction |
| Q-04 | Android phone scope at launch | Responsive viewer/editor fallback, primary tablet landscape |
| Q-05 | Precise advanced-electrical level | Properties of conductors/protection; not conductor-level CAD |
| Q-06 | Which EG4 models, portable products, verified spec revisions | Start with small documented model list |
| Q-07 | Weather API provider and forecast caching lifecycle | Offline presets first, optional opt-in adapter later |
| Q-08 | Multi-inverter AC coupling/parallel/phase handling | Treat unsupported configurations as unsupported, not simulated |
| Q-09 | Battery chemistry/charge curves and BMS behavior model | Explicit simplified efficiency, SOC limits and voltage ranges |
| Q-10 | Grid pricing, cost estimates and procurement workflow | Out of energy MVP; possible future module |
| Q-11 | Neutral/ground rule coverage by wiring mode | Conservative warnings + rule provenance, no certification |
| Q-12 | Product name, license, brand-image use policy | Working name; original/permissioned assets only |
| Q-13 | Multi-diagram semantics: alternative designs or electrically connected sheets | Independent alternatives for first release |
| Q-14 | Undo/redo, snapping, batch selection implementation depth | Undo/redo and grid snapping in MVP-A |
| Q-15 | Forecasts vs synthetic generation for offline cached future | User-selectable source; mark stale data clearly |

# U — UI Design
## U1. Information architecture
**App shell:** Project switcher / project title | Save and connection status | Undo / Redo | Grid / Snap | Weather / Scenario | Report / Export.

**Primary studio:** Collapsible LEFT toolbox with categories, searchable catalog, and favorites; CENTER virtual spatial canvas with port labels and selectable power lines; RIGHT inspector tabbed Equipment / Connection / Scenario / Telemetry; BOTTOM collapsible Diagnostics terminal or Event History. A floating fit/zoom control works with touch. Panels must not obscure every node on a landscape tablet.

**Other screens/panels:** Project gallery; New Project / Import; Catalog/Custom Profile editor; Scenario builder/results; Report preview; Settings/Offline/About; Recovery/Storage health.

## U2. Required screens and user states
| View | Primary action | Empty/error behavior |
|---|---|---|
| Project gallery | Create/open/import | Demonstration/sample project, storage status |
| Editor canvas | Drop, move, connect, inspect | Helpful new-diagram prompt; recover invalid connection |
| Equipment inspector | Edit name/spec/ports | Unknown specs conspicuously identified |
| Scenario editor | Choose weather/grid/load timeline | Missing battery or missing topology explained |
| Simulation results | Chart battery SOC and power flows | No misleading chart when network has no source |
| Diagnostics | Inspect severity and affected edge/node | No issues does not mean certified safe |
| Reports | Preview, print, export | Missing/estimated specs and model version in output |

## U3. Visual language
- Dark technical studio with subtle blueprint/spatial grid, high-contrast typography, restrained colored wires, and generous touch targets.
- Node types: **compact icon** (relay, switch), **standard card** (battery/load/array), **expanded hardware card** (hybrid inverter); consistent ports, IDs and status behavior in every rendering.
- Status values are distinct from electrical-flow color and have text/icon support: normal, disconnected, disabled, incompatible, warning, unknown.
- Animations are optional, throttled, and never the only indicator of an actual valid power path.
- Avoid unlicensed vendor logos/artwork until rights and use policy are verified; generic silhouettes/icons work offline.

## U4. Interaction rules and tablet cases
- Touch drag must not pan accidentally when initiated from a node or port; provide explicit gesture priority and touch hit zones.
- Pinch zoom centers on gesture focal point; zoom does not alter saved physical layout.
- Provide simple fallback tap-to-add/connect method for precise work with finger/stylus.
- Toggle grid visibility independently of snapping; toggling snapping must not teleport existing nodes.
- Deleting nodes prompts when it disconnects many edges; allow reversal with undo.
- Inspector edits use units and numeric validation; update diagram only after acceptable input.
- Canvas supports both sparse and dense graphs; render offscreen content efficiently, preserving exact world coordinates.
- Report should be understandable in black-and-white print and legible when zoomed.

## U5. Walkthrough test scripts
**UAT-01:** On an offline tablet, create project -> add array -> add battery/inverter -> wire -> edit load -> observe saved state -> reload -> verify coordinates and links persist.

**UAT-02:** Connect incompatible battery voltage -> see blocked edge plus explanation -> select compatible model -> connect; observe warnings for unresolved parameters.

**UAT-03:** Configure timed heater across midnight; simulate grid failure and a multi-day storm; graph, earliest outage timestamp and unmet load match engine results.

**UAT-04:** Export project, start an empty browser profile, import, and compare node counts, edge endpoints, custom catalog entries, weather scenarios and simulated outputs.

# L — Logic Design
## L1. Architecture (proposed)
**Static PWA host** serves app files and optional cached catalog. **Blazor WASM UI** manages interactions and calls pure C# application services. A **domain graph** owns node/port/edge semantics. A **validation pipeline** checks model invariants and simulation preconditions. The **simulation engine** uses deterministic time steps. **Local persistence** uses a browser storage adapter, and a **document/report service** produces local printable output. Optional **weather adapter** can fetch external data only when requested/allowed; simulation remains fully local.

Keep the simulation core independent from browser APIs, UI framework, storage mechanism and network APIs. Adopt explicit versions for simulation algorithm, catalog profile and project format.

## L2. Electrical system representation
- Each node has stable ID, profile reference/snapshot, world coordinates and named typed ports.
- Ports declare domain (DC / AC / signal/control), direction, voltage/current/power ranges, phase/frequency where applicable, and functional purpose (PV, battery, grid, output, bus, protective device).
- Edges connect port IDs and represent a supported electrical path; optional length/gauge/protection data only when calculations support them.
- For every snapshot/scenario state, traverse **connected eligible paths**; exclude visually nearby but unconnected nodes.
- Mark unsupported topology **UNKNOWN / NOT MODELED**, never silently approximate parallel generators, AC backfeed, transfer interlocks or inverter synchronization.
- Dynamic bond/neutral/ground properties depend on modes and topology. Basic MVP warns about potential conflicting bonds and unknown source behavior; it cannot establish legal compliance by boolean counting.

## L3. Calculation contracts and units
- **Nameplate DC power (W):** number of panels × nameplate watts per panel. Actual modeled available DC PV power depends on a *time-varying* effective sun/irradiance factor, array design, temperature, shade and derates as supported.
- **Energy (Wh):** P(W) × Δt(hours). **SOC (%):** 100 × Estored(Wh)/Enominal(Wh). Maintain battery energy in Wh throughout; never add raw watts to a percent.
- **Served vs demanded load:** account for inverter losses, idle loads, power source limits and outage states. When insufficient power, show **unmet load** rather than drawing negative battery energy.
- **Battery flows:** E_next = clamp[E_min, E_max](E_now + ηcharge·Pcharge·Δt − (Pdischarge/ηdischarge)·Δt), with charge/discharge efficiency assumptions declared and non-simultaneous net flow modeled explicitly.
- **Capacity floor:** Low-SOC cutoff is **configurable per equipment/system** (20% is an illustrative scenario default, not a universal inverter behavior). When cutoff triggers, update supply, served loads and any restart/recovery conditions consistently.
- **PV/battery/inverter distinctions:** Validate battery input against operating range; PV strings against MPPT startup, operating and open-circuit limits; grid AC input separately against frequency/voltage/phase and switching configuration.
- **DC vs AC:** Distinguish PV DC, battery DC, inverter AC, continuous watts, apparent VA where relevant, surge and power-factor unknowns. Do not sum currents across unrelated voltage buses or AC phases.
- **Weather modeling:** A fixed 15% overcast multiplier is an example mode, not meteorological truth. Hourly daylight must be zero at night. Seasonal location/daylength may be estimated offline using inputs; cache and random seeds are explicit.

## L4. Deterministic simulation cycle
1. Validate graph, profile completeness, selected scenario and simulation horizon.
2. Build time-indexed weather/irradiance, schedule and source availability timeline with timezone and offset rules specified.
3. For each time step, evaluate control/transfer modes and energized graph; calculate available PV and external sources, demand and idle overhead.
4. Apply defined dispatch priority, source power limits, inverter conversion and storage bounds. Track curtailment and unmet demand.
5. Update stored battery energy using Wh, charge/discharge efficiencies and configured limits; set fault/disabled/recovery statuses.
6. Record timestamp, step index, solar/grid/gen contributions, battery charge/discharge, load served/unmet, SOC, protection states and diagnostic events.
7. Derive summary: minimum SOC, total served and unmet Wh, first failure, affected loads and potential remedial what-if options **without guaranteeing a prescribed battery count**.
8. Render result from immutable run snapshot; if diagram changes, mark prior run stale until rerun.

**Simulation time behavior:** v1 defaults to hourly steps. Events crossing a step boundary are approximated at the defined resolution; display hour-level timestamps, not fake minute precision. Further resolution can be added only with deterministic tests.

## L5. Failure, validation, and edge-case matrix
| Case | Expected behavior |
|---|---|
| Zero panels, no sunlight, nighttime | PV=0; no impossible generation |
| Incompatible 201.6 V nominal pack to 48 V-class battery port | Block if outside verified full range; explain why |
| Incorrect PV string at cold-weather open-circuit voltage | Flag range conflict if source data exists; unknown if missing |
| Two potentially bonded sources / transfer configuration | Conservative topology-dependent warning; not simple global count |
| No transfer/isolation device in grid-interactive path | Warning or unsupported topology; no assertion of safe backfeed |
| Load exceeds inverter continuous capability | Warn and apply modeled served/unmet cap |
| Surge load or power factor missing | Mark as unknown limitation rather than falsely precise |
| SOC hits cutoff | Stop supported discharge; track unmet load, no negative energy |
| Battery charges above ceiling | Curtail overflow; no SOC above configured maximum |
| Timer crossing midnight | Correct daily evaluation and timezone/day transitions |
| Local DST change during scheduled scenario | Defined time conversion semantics; covered by tests |
| Cached weather expired | Display stale and offer fallback manual/preset source |
| Random disaster weather | Seeded output reruns identically |
| Grid drops, generator absent | Transition to permitted island mode only if equipment profile supports it |
| Disconnected load | Not counted as energized load |
| Circular wiring / direct source short | Prevent invalid direct path if modeled; otherwise critical not-modeled warning |
| Multiple battery banks with disparate chemistry | Enforce supported profiles or refuse/flag unsupported connection |
| Import malformed, huge or later schema | Reject safely with data intact; explain problem |
| Browser storage evicted / quota exceeded | Visible error, export recovery option |
| Branded profile missing specification | Unknown/research-needed label; never invented limits |
| System demand during power interruption | Record energy actually served, not demanded as if supplied |

## L6. Test strategy and quality gates
**Unit tests:** Electrical/energy formulas, boundary limits, charge/discharge efficiencies, timers, outages, AC/DC type constraints, deterministic seeded weather, graph connectivity and diagnosis IDs.

**Property tests/invariants:** 0 ≤ SOC ≤ configured maximum; served demand ≤ demanded demand; energy flow accounted for within declared tolerance; invalid hard connections never enter graph; random weather seed reproducibility; import/export preserves referential integrity.

**Integration tests:** UI edit -> domain graph -> diagnostics -> simulation -> persistence -> restore -> report. Offline service-worker tests include first install online and subsequent fully offline reload. Optional weather failure cannot block editing.

**Field acceptance:** Actual Android tablet with stylus and finger testing; browser desktop; landscape/portrait behavior; large multi-node graph; PWA update and storage resilience. Performance numbers are targets until benchmarked.

## L7. Development execution — release-first plan
**Gate 0: Design baseline.** Confirm open decisions that block storage, domain graph, weather semantics and first supported hardware. Create annotated wireframes, sample graph JSON and test fixtures.

**Gate 1: Deployed vertical slice.** Static-host PWA publicly accessible; offline shell; one generic array, inverter, battery and load; connect and move nodes; minimal reliable save; one valid scenario and one test. **Ship this early**.

**Gate 2: Complete circuit.** Grid, panel, relay/timer, source availability, power calculations, faults, multiple diagrams, import/export and core graph invariants.

**Gate 3: Scenario + evidence.** Weather presets, 30-day time stepping, power/SOC charts, diagnostics, reproducible Ice Storm sample, print-ready report, broader tests.

**Gate 4: Contractor readiness / MVP-A release.** Tablet polish, custom profiles, small verified EG4 library, offline clean install, error/recovery, accessibility, documentation, release notes and production smoke test.

**Anti-stall policy:** Every gate has a demonstrable published artifact and test checklist. No new catalog brand, weather API, cosmetic animation or future integration can interrupt a gate already in progress. Keep a visible backlog and a single next release target. No rewriting the stack without a measured blocker.

# D — Data Design
## D1. Domain entities (proposed schema)
| Entity | Essential fields | Notes |
|---|---|---|
| Project | id, name, createdAt, modifiedAt, schemaVersion | Top-level portable aggregate |
| Diagram | id, projectId, name, nodes[], edges[], viewport | Alternative design unless explicitly linked |
| NodeInstance | id, profileId, profileSnapshot, x, y, displayMode, parameters | Stable IDs; appearance not electrical truth |
| PortDefinition | id, ownerId, label, electricalDomain, role, direction, limits | Typed endpoint |
| Edge | id, fromNodeId, fromPortId, toNodeId, toPortId, properties | Full referential integrity |
| EquipmentProfile | id, manufacturer, model, revision, category, source, confidence, limits | Built-in or user-defined |
| BatteryConfig | nominalWh, minSoc, maxSoc, initialSoc, charge/discharge limits, efficiencies | Distinguish nominal vs usable |
| LoadProfile | activeW, idleW, dutyCycle, schedule, surgeW?, powerFactor? | Schedule as explicit local-time rules |
| EnvironmentProfile | id, name, locationInputs?, weatherSource, timeSeries, seed?, updatedAt? | Nullable location; no location requirement |
| SimulationScenario | id, diagramId, start, duration, stepMinutes, sourceOutages, scheduleOverrides, environmentId | Immutable run inputs snapshot |
| SimulationRun | id, scenarioId, inputHash, engineVersion, points[], events[], summary, createdAt | Stale on source edits |
| Diagnostic | id, ruleId, severity, nodeIds[], edgeIds[], message, assumptions, reference? | Rule provenance and repeatability |
| AppPreferences | theme, gridVisible, snapEnabled, zoomBehavior, accessibility, offlineMode | Local to device |

## D2. Persistence, portability and migrations
- **PROPOSED:** IndexedDB is the authoritative local project store, accessed through a narrow repository interface. Browser localStorage may hold small UI preferences but not the canonical project graph. Evaluate quota/eviction and storage persistence support.
- Autosave debounces normal edits and flushes on appropriate lifecycle transitions without claiming guaranteed writes on power loss. User-facing status indicates pending/saved/error.
- Portable JSON includes a root `schemaVersion`, `appVersion`, `project`, diagrams, embedded custom equipment snapshots and scenarios. IDs must remain internally consistent after import.
- Import validates content type, size bounds, schema and graph references; preview conflicts and provide import-as-copy rather than destructive overwrite by default.
- Migrations are versioned and tested against fixture files from prior versions. Preserve unknown fields where feasible or communicate loss explicitly.
- Sensitive data: no required personal details, accounts or location; manually entered coordinates/location are project data unless explicitly exported or shared.
- Cache of internet weather is kept separate from equipment/project truth and tagged with source, retrieval time, forecast valid time and expiration.

## D3. Example portable JSON skeleton (illustrative, not final contract)
```json
{
  "schemaVersion": 1,
  "appVersion": "0.1.0",
  "project": {
    "id": "project-uuid",
    "name": "Goat Barn Solar",
    "diagrams": [{ "id": "diagram-uuid", "name": "Baseline", "nodes": [], "edges": [] }],
    "scenarios": []
  }
}
```

## D4. Source of truth and recalculation
- Saved nodes/edges and selected equipment profile data are canonical. Derived watts, SOC, diagnostics, simulation plots and energized wire states are recalculated from a stable model snapshot, not stored as editable authority.
- Project import/export portability does not depend on separately installed brand profile packs: include necessary profile snapshots, version identifiers and provenance.
- Store simulation results optionally for convenience, tagged with input hash. If inputs differ, show **stale result** and require rerun.

# Release acceptance contract
The MVP-A can be declared ready for a private beta only when **all** conditions below are demonstrated:
1. Private-beta URL exists and PWA loads from static hosting; after installation, a network-disabled fresh restart permits full design/save/simulate/report flow.
2. On a real Android tablet, user creates diagram with PV, inverter, battery, grid, panel and scheduled load, wires/edits and pans/zooms without losing connections.
3. Strict mismatched port/battery voltage test blocks known incompatible connection and gives traceable explanation.
4. Energy engine passes unit/invariant tests and reproduces a seeded 30-day outage; event timestamps match configured step resolution.
5. Autosave survives reload; JSON export/import preserves all diagram topology, profile snapshots and scenarios; invalid files cannot damage existing projects.
6. Printed report contains equipment, assumptions, run conditions, results, important diagnostics and non-certification disclaimer.
7. Catalog differentiates verified brand specifications from generic/user estimated values; absent data is labeled UNKNOWN.
8. Tests pass in CI, private-beta release notes and known limitations are shared with invited testers, and no blocking defects remain for the core workflow.

# Open decision register — next review meeting
**Priority 1 (architecture blockers):** Q-01 Blazor/canvas approach; Q-02 library spike; Q-03 persistence adapter; Q-13 whether multiple diagrams are alternatives.  
**Priority 2 (engineering correctness):** Q-05 detail level; Q-06 first verified hardware; Q-08 multi-inverter handling; Q-09 battery model; Q-11 bonding rule scope.  
**Priority 3 (delivery and polish):** Q-04 phone support; Q-07 weather API; Q-10 costs; Q-12 logo/license; Q-14 editing conveniences; Q-15 cached forecast fallback.

**Companion implementation guidance (DRAFT):** `SKILLS.md`, `.github/copilot-instructions.md`, and a task-specific `.github/skills/solar-sandbox-implementation/SKILL.md` have been drafted for review. They reference this master, do not override its requirements, and will be finalized once the remaining technical decisions are approved.

# Provenance and framework note
- Original uploaded user specification: *MVP Specification: Off-Grid Topology Tracer (Solar Sandbox)*, October 29, 2026 conversation attachment. Examples and duplicated paragraphs were consolidated.
- User confirmations: Round 1 (6 decisions), Round 2 (7 decisions), subsequent weather/canvas/catalog/visual refinements.
- W.O.U.L.D. expansion follows Tim Corey's teaching: Walk Through It; Open Up the Requirements; UI Design; Logic Design; Data Design. The stage structure is verified from educational references; this document is an original Solar Sandbox adaptation, not an official Tim Corey template.
