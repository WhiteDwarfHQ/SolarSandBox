namespace SolarSandBox.UI.Models;

public sealed class SolarProjectDocument
{
    public required string Id { get; init; }

    public int SchemaVersion { get; init; } = 1;

    public required string AppVersion { get; init; }

    public required StartupProject Project { get; init; }

    public required List<ProjectNodePlaceholder> Nodes { get; init; }
}
