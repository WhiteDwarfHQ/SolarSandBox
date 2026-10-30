namespace SolarSandBox.UI.Domain;

public sealed class EquipmentInstance
{
    public required string Id { get; init; }

    public required string DefinitionId { get; init; }

    public required string DisplayName { get; init; }

    public string? DiagramNodeId { get; init; }

    public Dictionary<string, string> Configuration { get; init; } = [];

    public SpecificationVerificationStatus VerificationStatus { get; init; } = SpecificationVerificationStatus.Unknown;
}
