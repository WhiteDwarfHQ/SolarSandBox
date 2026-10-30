namespace SolarSandBox.UI.Domain;

public sealed class ConnectionValidationResult
{
    public required bool IsAllowed { get; init; }

    public required bool IsProvisional { get; init; }

    public required List<ConnectionValidationMessage> Messages { get; init; }

    public static ConnectionValidationResult Allowed(bool provisional, params ConnectionValidationMessage[] messages)
    {
        return new ConnectionValidationResult
        {
            IsAllowed = true,
            IsProvisional = provisional,
            Messages = [.. messages]
        };
    }

    public static ConnectionValidationResult Rejected(params ConnectionValidationMessage[] messages)
    {
        return new ConnectionValidationResult
        {
            IsAllowed = false,
            IsProvisional = false,
            Messages = [.. messages]
        };
    }
}
