using Blazor.Diagrams;
using Blazor.Diagrams.Core.Models;
using SolarSandBox.UI.Domain;

namespace SolarSandBox.UI.Services;

public sealed class CanvasDiagramBuildResult
{
    public required BlazorDiagram Diagram { get; init; }

    public required Dictionary<string, EquipmentPortReference> PortBindingsByDiagramPortId { get; init; }

    public required Dictionary<string, CanvasTerminalDescriptor> TerminalByDiagramPortId { get; init; }

    public required Dictionary<string, EquipmentDefinition> DefinitionsById { get; init; }

    public required Dictionary<string, EquipmentInstance> InstancesById { get; init; }

    public required int NodeCount { get; init; }

    public required int LinkCount { get; init; }
}
