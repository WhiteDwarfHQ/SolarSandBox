namespace SolarSandBox.UI.Services;

public sealed class CanvasTerminalDescriptor
{
    public required string PortId { get; init; }

    public required string TerminalCode { get; init; }

    public required string DisplayLabel { get; init; }

    public required string PositionHint { get; init; }

    public required int SortOrder { get; init; }
}
