using SolarSandBox.UI.Domain;

namespace SolarSandBox.UI.Services;

public sealed class EquipmentInspectorState
{
    public required string EquipmentInstanceId { get; init; }

    public required string Title { get; init; }

    public required string Subtitle { get; init; }

    public required string HeroLine { get; init; }

    public required string ConnectionsSummary { get; init; }

    public required EquipmentCategory Category { get; init; }

    public required IReadOnlyList<InspectorConnectionLine> ConnectionLines { get; init; }
}

public sealed class InspectorConnectionLine
{
    public required string Name { get; init; }

    public required string Detail { get; init; }

    public required bool Connected { get; init; }
}
