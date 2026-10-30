using Blazor.Diagrams;
using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Options;
using SolarSandBox.UI.Domain;
using SolarSandBox.UI.Models;

namespace SolarSandBox.UI.Services;

public sealed class CanvasDiagramAdapter : ICanvasDiagramAdapter
{
    public CanvasDiagramBuildResult Build(SolarProjectDocument document)
    {
        var options = new BlazorDiagramOptions
        {
            AllowPanning = true,
            GridSize = 24,
            Zoom =
            {
                Enabled = true,
                Minimum = 0.25,
                Maximum = 3.0,
                ScaleFactor = 1.1
            }
        };

        var diagram = new BlazorDiagram(options, registerDefaultBehaviors: true);

        var definitionsById = document.EquipmentDefinitions.ToDictionary(x => x.Id, StringComparer.Ordinal);
        var instancesById = document.EquipmentInstances.ToDictionary(x => x.Id, StringComparer.Ordinal);
        var instanceToNode = new Dictionary<string, NodeModel>(StringComparer.Ordinal);
        var portBindings = new Dictionary<string, EquipmentPortReference>(StringComparer.Ordinal);
        var terminalsByDiagramPortId = new Dictionary<string, CanvasTerminalDescriptor>(StringComparer.Ordinal);

        foreach (var instance in document.EquipmentInstances)
        {
            var placeholder = document.Nodes.FirstOrDefault(n => n.Id == (instance.DiagramNodeId ?? instance.Id))
                ?? document.Nodes.FirstOrDefault(n => n.Label == instance.DisplayName)
                ?? document.Nodes.FirstOrDefault();

            if (!definitionsById.TryGetValue(instance.DefinitionId, out var definition))
            {
                continue;
            }

            var (roleTitle, metaLine, footerLine) = BuildCardContent(definition);
            var terminals = BuildTerminalDescriptors(definition).ToList();

            var node = diagram.Nodes.Add(new CanvasEquipmentNodeModel(instance.Id, new Point(placeholder?.X ?? 140, placeholder?.Y ?? 140))
            {
                Title = instance.DisplayName,
                EquipmentInstanceId = instance.Id,
                DefinitionId = instance.DefinitionId,
                Category = definition.Category,
                RoleTitle = roleTitle,
                MetaLine = metaLine,
                FooterLine = footerLine,
                Terminals = terminals
            });

            instanceToNode[instance.Id] = node;

            foreach (var terminal in terminals)
            {
                var portDefinition = definition.Ports.FirstOrDefault(p => p.Id == terminal.PortId);
                if (portDefinition is null)
                {
                    continue;
                }

                var diagramPortId = $"{instance.Id}::{portDefinition.Id}";
                var alignment = MapAlignment(portDefinition, terminal);

                var port = node.AddPort(new PortModel(diagramPortId, node, alignment, new Point(0, 0), new Size(20, 20)));

                portBindings[port.Id] = new EquipmentPortReference
                {
                    EquipmentInstanceId = instance.Id,
                    PortId = portDefinition.Id
                };

                terminalsByDiagramPortId[port.Id] = terminal;
            }
        }

        foreach (var connection in document.ElectricalConnections)
        {
            var fromDiagramPortId = $"{connection.From.EquipmentInstanceId}::{connection.From.PortId}";
            var toDiagramPortId = $"{connection.To.EquipmentInstanceId}::{connection.To.PortId}";

            if (!portBindings.ContainsKey(fromDiagramPortId) || !portBindings.ContainsKey(toDiagramPortId))
            {
                continue;
            }

            if (!TryGetPort(instanceToNode, connection.From, out var fromPort) || !TryGetPort(instanceToNode, connection.To, out var toPort))
            {
                continue;
            }

            var visual = BuildConnectionVisual(connection, instancesById, definitionsById);

            var link = diagram.Links.Add(new LinkModel(connection.Id, new SinglePortAnchor(fromPort), new SinglePortAnchor(toPort))
            {
                Color = visual.Color,
                Width = visual.Width
            });

            link.Refresh();
        }

        return new CanvasDiagramBuildResult
        {
            Diagram = diagram,
            PortBindingsByDiagramPortId = portBindings,
            TerminalByDiagramPortId = terminalsByDiagramPortId,
            DefinitionsById = definitionsById,
            InstancesById = instancesById,
            NodeCount = diagram.Nodes.Count,
            LinkCount = diagram.Links.Count
        };
    }

    private static bool TryGetPort(Dictionary<string, NodeModel> instanceToNode, EquipmentPortReference reference, out PortModel port)
    {
        if (instanceToNode.TryGetValue(reference.EquipmentInstanceId, out var node))
        {
            var id = $"{reference.EquipmentInstanceId}::{reference.PortId}";
            var existing = node.Ports.FirstOrDefault(p => p.Id == id);
            if (existing is not null)
            {
                port = existing;
                return true;
            }
        }

        port = default!;
        return false;
    }

    private static PortAlignment MapAlignment(ElectricalPortDefinition port, CanvasTerminalDescriptor terminal)
    {
        if (terminal.PositionHint.Equals("top", StringComparison.OrdinalIgnoreCase))
        {
            return PortAlignment.Top;
        }

        if (terminal.PositionHint.Equals("left", StringComparison.OrdinalIgnoreCase))
        {
            return PortAlignment.Left;
        }

        if (terminal.PositionHint.Equals("right", StringComparison.OrdinalIgnoreCase))
        {
            return PortAlignment.Right;
        }

        return port.Purpose switch
        {
            PortPurpose.PvInput => PortAlignment.Top,
            PortPurpose.PvOutput => PortAlignment.Bottom,
            PortPurpose.BatteryConnection => port.Direction == PortDirection.Input ? PortAlignment.Left : PortAlignment.Right,
            PortPurpose.AcInput => PortAlignment.Left,
            PortPurpose.AcOutput => PortAlignment.Right,
            PortPurpose.LoadInput => PortAlignment.Left,
            _ => port.Direction switch
            {
                PortDirection.Input => PortAlignment.Left,
                PortDirection.Output => PortAlignment.Right,
                _ => PortAlignment.Bottom
            }
        };
    }

    private static (string RoleTitle, string MetaLine, string FooterLine) BuildCardContent(EquipmentDefinition definition)
    {
        return definition.Category switch
        {
            EquipmentCategory.SolarArray => ("PV Generation", "Array Segment", "PV Output"),
            EquipmentCategory.BatteryBank => ("Energy Storage", "DC Battery Bank", "Battery Port"),
            EquipmentCategory.Inverter => ("AC Distribution", "Hybrid Inverter", "AC / DC Ports"),
            EquipmentCategory.ElectricalLoad => ("Critical Load", "Load Endpoint", "Load Input"),
            _ => ("Electrical Device", "Conceptual Device", "Port Group")
        };
    }

    private static IEnumerable<CanvasTerminalDescriptor> BuildTerminalDescriptors(EquipmentDefinition definition)
    {
        return definition.Ports
            .Select(port => new CanvasTerminalDescriptor
            {
                PortId = port.Id,
                TerminalCode = BuildTerminalCode(port),
                DisplayLabel = BuildDisplayLabel(port),
                PositionHint = BuildPositionHint(definition.Category, port),
                SortOrder = BuildSortOrder(definition.Category, port)
            })
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.DisplayLabel, StringComparer.Ordinal)
            .ToList();
    }

    private static string BuildTerminalCode(ElectricalPortDefinition port)
    {
        return port.Purpose switch
        {
            PortPurpose.PvInput => "PV IN",
            PortPurpose.PvOutput => "PV OUT",
            PortPurpose.BatteryConnection => "BAT",
            PortPurpose.AcInput => "GRID IN",
            PortPurpose.AcOutput => "AC OUT",
            PortPurpose.LoadInput => "LOAD IN",
            _ => port.Direction switch
            {
                PortDirection.Input => "IN",
                PortDirection.Output => "OUT",
                _ => "I/O"
            }
        };
    }

    private static string BuildDisplayLabel(ElectricalPortDefinition port)
    {
        return port.Purpose switch
        {
            PortPurpose.PvInput => "PV Input",
            PortPurpose.PvOutput => "PV Output",
            PortPurpose.BatteryConnection => "Battery DC",
            PortPurpose.AcInput => "Grid AC Input",
            PortPurpose.AcOutput => "AC Output",
            PortPurpose.LoadInput => "Load AC Input",
            _ => port.Label
        };
    }

    private static string BuildPositionHint(EquipmentCategory category, ElectricalPortDefinition port)
    {
        return category switch
        {
            EquipmentCategory.SolarArray => "bottom",
            EquipmentCategory.BatteryBank => "left",
            EquipmentCategory.Inverter => port.Purpose switch
            {
                PortPurpose.PvInput => "top",
                PortPurpose.BatteryConnection => "left",
                PortPurpose.AcOutput => "right",
                PortPurpose.AcInput => "left",
                _ => "right"
            },
            EquipmentCategory.ElectricalLoad => "left",
            _ => port.Direction == PortDirection.Output ? "right" : "left"
        };
    }

    private static int BuildSortOrder(EquipmentCategory category, ElectricalPortDefinition port)
    {
        return category switch
        {
            EquipmentCategory.Inverter => port.Purpose switch
            {
                PortPurpose.PvInput => 10,
                PortPurpose.BatteryConnection => 20,
                PortPurpose.AcInput => 30,
                PortPurpose.AcOutput => 40,
                _ => 90
            },
            EquipmentCategory.SolarArray => 10,
            EquipmentCategory.BatteryBank => 10,
            EquipmentCategory.ElectricalLoad => 10,
            _ => 99
        };
    }

    private static (string Color, double Width, string Label) BuildConnectionVisual(
        ElectricalConnection connection,
        IReadOnlyDictionary<string, EquipmentInstance> instancesById,
        IReadOnlyDictionary<string, EquipmentDefinition> definitionsById)
    {
        var type = ResolveElectricalType(connection, instancesById, definitionsById);

        if (type == ElectricalType.Ac)
        {
            return connection.IsVerified
                ? ("#36c2ff", 3.2, "AC")
                : ("#36c2ff", 2.8, "AC · visual");
        }

        if (type == ElectricalType.Dc)
        {
            return connection.IsVerified
                ? ("#f9af1c", 3.2, "DC")
                : ("#f9af1c", 2.8, "DC · visual");
        }

        return connection.IsVerified
            ? ("#c6d2f8", 3.0, "UNK")
            : ("#a6b2d6", 2.6, "UNK · visual");
    }

    private static ElectricalType ResolveElectricalType(
        ElectricalConnection connection,
        IReadOnlyDictionary<string, EquipmentInstance> instancesById,
        IReadOnlyDictionary<string, EquipmentDefinition> definitionsById)
    {
        var from = ResolvePort(connection.From, instancesById, definitionsById);
        var to = ResolvePort(connection.To, instancesById, definitionsById);

        if (from is null || to is null)
        {
            return ElectricalType.Unknown;
        }

        return from.ElectricalType == to.ElectricalType ? from.ElectricalType : ElectricalType.Unknown;
    }

    private static ElectricalPortDefinition? ResolvePort(
        EquipmentPortReference reference,
        IReadOnlyDictionary<string, EquipmentInstance> instancesById,
        IReadOnlyDictionary<string, EquipmentDefinition> definitionsById)
    {
        if (!instancesById.TryGetValue(reference.EquipmentInstanceId, out var instance))
        {
            return null;
        }

        if (!definitionsById.TryGetValue(instance.DefinitionId, out var definition))
        {
            return null;
        }

        return definition.Ports.FirstOrDefault(x => x.Id == reference.PortId);
    }
}
