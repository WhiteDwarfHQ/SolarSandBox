namespace SolarSandBox.UI.Domain;

public sealed class ElectricalConnection
{
    public required string Id { get; init; }

    public required EquipmentPortReference From { get; init; }

    public required EquipmentPortReference To { get; init; }

    public bool IsVerified { get; init; }

    public string? VerificationNotes { get; init; }
}
