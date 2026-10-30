namespace SolarSandBox.UI.Domain;

public sealed class ElectricalPortDefinition
{
    public required string Id { get; init; }

    public required string Label { get; init; }

    public required ElectricalType ElectricalType { get; init; }

    public required PortDirection Direction { get; init; }

    public required PortPurpose Purpose { get; init; }

    public double? VoltageMin { get; init; }

    public double? VoltageMax { get; init; }

    public double? CurrentMax { get; init; }

    public SpecificationVerificationStatus VerificationStatus { get; init; } = SpecificationVerificationStatus.Unknown;
}
