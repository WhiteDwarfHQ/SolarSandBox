namespace SolarSandBox.UI.Domain;

public sealed class ConnectionValidationMessage
{
    public required string Code { get; init; }

    public required DiagnosticSeverity Severity { get; init; }

    public required string Description { get; init; }
}
