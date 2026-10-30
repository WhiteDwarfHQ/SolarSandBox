namespace SolarSandBox.UI.Domain;

public sealed class EquipmentDefinition
{
    public required string Id { get; init; }

    public required EquipmentCategory Category { get; init; }

    public required string DisplayName { get; init; }

    public string? Manufacturer { get; init; }

    public string? Model { get; init; }

    public SpecificationVerificationStatus VerificationStatus { get; init; } = SpecificationVerificationStatus.Unknown;

    public Dictionary<string, string> Configuration { get; init; } = [];

    public List<ElectricalPortDefinition> Ports { get; init; } = [];
}
