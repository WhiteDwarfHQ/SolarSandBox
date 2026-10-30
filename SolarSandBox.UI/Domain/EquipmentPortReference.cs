namespace SolarSandBox.UI.Domain;

public sealed class EquipmentPortReference
{
    public required string EquipmentInstanceId { get; init; }

    public required string PortId { get; init; }
}
