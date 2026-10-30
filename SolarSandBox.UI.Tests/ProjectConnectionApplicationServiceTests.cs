using SolarSandBox.UI.Domain;
using SolarSandBox.UI.Models;
using SolarSandBox.UI.Services;

namespace SolarSandBox.UI.Tests;

public sealed class ProjectConnectionApplicationServiceTests
{
    [Fact]
    public async Task TryCreateConnectionAsync_WhenValid_PersistsConnection()
    {
        var repository = new InMemoryStartupProjectRepository(CreateDocument());
        var service = new ProjectConnectionApplicationService(repository, new ConnectionValidationService());

        var result = await service.TryCreateConnectionAsync(new ElectricalConnection
        {
            Id = "conn-new",
            From = new EquipmentPortReference
            {
                EquipmentInstanceId = "inst-inverter",
                PortId = "battery-dc-inout"
            },
            To = new EquipmentPortReference
            {
                EquipmentInstanceId = "inst-battery",
                PortId = "battery-dc-port"
            },
            IsVerified = false
        });

        Assert.True(result.IsAllowed);
        Assert.Contains(repository.Document!.ElectricalConnections, x => x.Id == "conn-new");
    }

    [Fact]
    public async Task TryCreateConnectionAsync_WhenInvalid_DoesNotPersistConnection()
    {
        var repository = new InMemoryStartupProjectRepository(CreateDocument());
        var service = new ProjectConnectionApplicationService(repository, new ConnectionValidationService());

        var result = await service.TryCreateConnectionAsync(new ElectricalConnection
        {
            Id = "conn-bad",
            From = new EquipmentPortReference
            {
                EquipmentInstanceId = "inst-pv",
                PortId = "pv-dc-out"
            },
            To = new EquipmentPortReference
            {
                EquipmentInstanceId = "inst-load",
                PortId = "ac-load-in"
            },
            IsVerified = false
        });

        Assert.False(result.IsAllowed);
        Assert.DoesNotContain(repository.Document!.ElectricalConnections, x => x.Id == "conn-bad");
    }

    private static SolarProjectDocument CreateDocument()
    {
        var project = new StartupProject
        {
            Id = "project-1",
            Name = "Demo",
            Kind = StartupProjectKind.DemoSystem,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow
        };

        return new SolarProjectDocument
        {
            Id = project.Id,
            SchemaVersion = 2,
            AppVersion = "0.1.0",
            Project = project,
            Nodes = [],
            EquipmentDefinitions = GenericEquipmentCatalog.CreateStarterDefinitions(),
            EquipmentInstances =
            [
                new EquipmentInstance { Id = "inst-pv", DefinitionId = "def-solar-array-6x400w", DisplayName = "PV" },
                new EquipmentInstance { Id = "inst-battery", DefinitionId = "def-battery-48v-generic", DisplayName = "Battery" },
                new EquipmentInstance { Id = "inst-inverter", DefinitionId = "def-inverter-generic", DisplayName = "Inverter" },
                new EquipmentInstance { Id = "inst-load", DefinitionId = "def-load-generic", DisplayName = "Load" }
            ],
            ElectricalConnections = []
        };
    }

    private sealed class InMemoryStartupProjectRepository(SolarProjectDocument document) : IStartupProjectRepository
    {
        public SolarProjectDocument? Document { get; private set; } = document;

        public Task InitializeAsync() => Task.CompletedTask;

        public Task<StartupProject?> GetLastActiveProjectAsync() => Task.FromResult(Document?.Project);

        public Task<StartupProject> CreateProjectAsync(StartupProjectKind kind) => throw new NotSupportedException();

        public Task SetLastActiveProjectAsync(string projectId) => Task.CompletedTask;

        public Task<SolarProjectDocument?> GetActiveProjectDocumentAsync() => Task.FromResult(Document);

        public Task SaveProjectDocumentAsync(SolarProjectDocument document)
        {
            Document = document;
            return Task.CompletedTask;
        }

        public Task<string?> ExportActiveProjectJsonAsync() => Task.FromResult<string?>(null);
    }
}
