using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using SolarSandBox.UI.Domain;

namespace SolarSandBox.UI.Services;

public sealed class CanvasEquipmentNodeModel : NodeModel
{
    public CanvasEquipmentNodeModel(string id, Point position)
        : base(id, position)
    {
    }

    public required string EquipmentInstanceId { get; init; }

    public required string DefinitionId { get; init; }

    public required EquipmentCategory Category { get; init; }

    public required string RoleTitle { get; init; }

    public required string MetaLine { get; init; }

    public required string FooterLine { get; init; }

    public IReadOnlyList<CanvasTerminalDescriptor> Terminals { get; init; } = [];

    public Func<CanvasEquipmentNodeModel, Task>? InspectRequestedAsync { get; set; }

    public Task RequestInspectAsync()
    {
        return InspectRequestedAsync?.Invoke(this) ?? Task.CompletedTask;
    }
}
