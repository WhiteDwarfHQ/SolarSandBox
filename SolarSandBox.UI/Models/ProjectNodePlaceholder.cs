namespace SolarSandBox.UI.Models;

public sealed class ProjectNodePlaceholder
{
    public required string Id { get; init; }

    public required string Kind { get; init; }

    public required string Label { get; init; }

    public double X { get; init; }

    public double Y { get; init; }
}
