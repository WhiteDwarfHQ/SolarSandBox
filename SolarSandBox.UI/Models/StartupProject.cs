namespace SolarSandBox.UI.Models;

public sealed class StartupProject
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public required StartupProjectKind Kind { get; init; }

    public DateTimeOffset CreatedAtUtc { get; init; }

    public DateTimeOffset UpdatedAtUtc { get; init; }
}
