using SolarSandBox.UI.Domain;

namespace SolarSandBox.UI.Models;

public sealed class SolarProjectDocument
{
    public required string Id { get; init; }

    public int SchemaVersion { get; init; } = 2;

    public required string AppVersion { get; init; }

    public required StartupProject Project { get; init; }

    public required List<ProjectNodePlaceholder> Nodes { get; init; }

    public List<EquipmentDefinition> EquipmentDefinitions { get; init; } = [];

    public List<EquipmentInstance> EquipmentInstances { get; init; } = [];

    public List<ElectricalConnection> ElectricalConnections { get; init; } = [];
}
