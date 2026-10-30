using SolarSandBox.UI.Domain;
using SolarSandBox.UI.Models;

namespace SolarSandBox.UI.Services;

public sealed class ProjectBootstrapFactory : IProjectBootstrapFactory
{
    public SolarProjectDocument CreateDocument(StartupProjectKind kind, DateTimeOffset createdAtUtc)
    {
        var projectId = Guid.NewGuid().ToString("N");
        var demo = kind == StartupProjectKind.DemoSystem;

        var definitions = GenericEquipmentCatalog.CreateStarterDefinitions();
        var instances = demo ? CreateDemoInstances(projectId) : [];
        var connections = demo ? CreateDemoConnections(projectId) : [];

        return new SolarProjectDocument
        {
            Id = projectId,
            AppVersion = "0.1.0",
            Project = new StartupProject
            {
                Id = projectId,
                Name = demo ? "Demo System" : "Blank Canvas",
                Kind = kind,
                CreatedAtUtc = createdAtUtc,
                UpdatedAtUtc = createdAtUtc
            },
            Nodes = demo
                ? CreateDemoNodes(projectId)
                : [],
            EquipmentDefinitions = definitions,
            EquipmentInstances = instances,
            ElectricalConnections = connections
        };
    }

    private static List<ProjectNodePlaceholder> CreateDemoNodes(string projectId)
    {
        return
        [
            new ProjectNodePlaceholder
            {
                Id = $"{projectId}-pv-array-01",
                Kind = "pv-array",
                Label = "PV Array A-01",
                X = 120,
                Y = 90
            },
            new ProjectNodePlaceholder
            {
                Id = $"{projectId}-battery-01",
                Kind = "battery",
                Label = "Battery Bank B-01",
                X = 220,
                Y = 260
            },
            new ProjectNodePlaceholder
            {
                Id = $"{projectId}-inverter-01",
                Kind = "inverter",
                Label = "Inverter I-01",
                X = 430,
                Y = 170
            },
            new ProjectNodePlaceholder
            {
                Id = $"{projectId}-load-01",
                Kind = "load",
                Label = "Essential Load Panel",
                X = 670,
                Y = 230
            }
        ];
    }

    private static List<EquipmentInstance> CreateDemoInstances(string projectId)
    {
        return
        [
            new EquipmentInstance
            {
                Id = $"{projectId}-inst-pv-01",
                DefinitionId = "def-solar-array-6x400w",
                DisplayName = "Demo PV Array",
                DiagramNodeId = $"{projectId}-pv-array-01",
                VerificationStatus = SpecificationVerificationStatus.UserProvided
            },
            new EquipmentInstance
            {
                Id = $"{projectId}-inst-battery-01",
                DefinitionId = "def-battery-48v-generic",
                DisplayName = "Demo Battery Bank",
                DiagramNodeId = $"{projectId}-battery-01",
                VerificationStatus = SpecificationVerificationStatus.UserProvided
            },
            new EquipmentInstance
            {
                Id = $"{projectId}-inst-inverter-01",
                DefinitionId = "def-inverter-generic",
                DisplayName = "Demo Inverter",
                DiagramNodeId = $"{projectId}-inverter-01",
                VerificationStatus = SpecificationVerificationStatus.UserProvided
            },
            new EquipmentInstance
            {
                Id = $"{projectId}-inst-load-01",
                DefinitionId = "def-load-generic",
                DisplayName = "Demo Load",
                DiagramNodeId = $"{projectId}-load-01",
                VerificationStatus = SpecificationVerificationStatus.Unknown
            }
        ];
    }

    private static List<ElectricalConnection> CreateDemoConnections(string projectId)
    {
        return
        [
            new ElectricalConnection
            {
                Id = $"{projectId}-conn-pv-inverter",
                From = new EquipmentPortReference
                {
                    EquipmentInstanceId = $"{projectId}-inst-pv-01",
                    PortId = "pv-dc-out"
                },
                To = new EquipmentPortReference
                {
                    EquipmentInstanceId = $"{projectId}-inst-inverter-01",
                    PortId = "pv-dc-in"
                },
                IsVerified = true,
                VerificationNotes = "Known compatible DC PV input path in demo catalog assumptions."
            },
            new ElectricalConnection
            {
                Id = $"{projectId}-conn-battery-inverter",
                From = new EquipmentPortReference
                {
                    EquipmentInstanceId = $"{projectId}-inst-battery-01",
                    PortId = "battery-dc-port"
                },
                To = new EquipmentPortReference
                {
                    EquipmentInstanceId = $"{projectId}-inst-inverter-01",
                    PortId = "battery-dc-inout"
                },
                IsVerified = true,
                VerificationNotes = "Known compatible 48V-class battery connection in demo assumptions."
            },
            new ElectricalConnection
            {
                Id = $"{projectId}-conn-inverter-load",
                From = new EquipmentPortReference
                {
                    EquipmentInstanceId = $"{projectId}-inst-inverter-01",
                    PortId = "ac-out"
                },
                To = new EquipmentPortReference
                {
                    EquipmentInstanceId = $"{projectId}-inst-load-01",
                    PortId = "ac-load-in"
                },
                IsVerified = false,
                VerificationNotes = "Load profile is generic and partially unverified; connection retained as provisional demo example."
            }
        ];
    }
}
