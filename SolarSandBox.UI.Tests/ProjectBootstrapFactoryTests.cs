using SolarSandBox.UI.Models;
using SolarSandBox.UI.Services;

namespace SolarSandBox.UI.Tests;

public sealed class ProjectBootstrapFactoryTests
{
    private readonly ProjectBootstrapFactory _factory = new();

    [Fact]
    public void CreateDocument_BlankCanvas_HasNoPlaceholderNodes()
    {
        var createdAt = new DateTimeOffset(2026, 10, 30, 0, 0, 0, TimeSpan.Zero);

        var document = _factory.CreateDocument(StartupProjectKind.BlankCanvas, createdAt);

        Assert.Equal(2, document.SchemaVersion);
        Assert.Equal("0.1.0", document.AppVersion);
        Assert.Equal(StartupProjectKind.BlankCanvas, document.Project.Kind);
        Assert.Equal(createdAt, document.Project.CreatedAtUtc);
        Assert.Empty(document.Nodes);
        Assert.Empty(document.EquipmentInstances);
        Assert.Empty(document.ElectricalConnections);
        Assert.Equal(4, document.EquipmentDefinitions.Count);
    }

    [Fact]
    public void CreateDocument_DemoSystem_HasMinimalPlaceholderNodes()
    {
        var createdAt = DateTimeOffset.UtcNow;

        var document = _factory.CreateDocument(StartupProjectKind.DemoSystem, createdAt);

        Assert.Equal(StartupProjectKind.DemoSystem, document.Project.Kind);
        Assert.Equal(4, document.Nodes.Count);
        Assert.Contains(document.Nodes, n => n.Kind == "pv-array");
        Assert.Contains(document.Nodes, n => n.Kind == "battery");
        Assert.Contains(document.Nodes, n => n.Kind == "inverter");
        Assert.Contains(document.Nodes, n => n.Kind == "load");
        Assert.Equal(4, document.EquipmentDefinitions.Count);
        Assert.Equal(4, document.EquipmentInstances.Count);
        Assert.Equal(3, document.ElectricalConnections.Count);
    }
}
