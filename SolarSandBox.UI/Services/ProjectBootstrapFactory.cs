using SolarSandBox.UI.Models;

namespace SolarSandBox.UI.Services;

public sealed class ProjectBootstrapFactory : IProjectBootstrapFactory
{
    public SolarProjectDocument CreateDocument(StartupProjectKind kind, DateTimeOffset createdAtUtc)
    {
        var projectId = Guid.NewGuid().ToString("N");

        return new SolarProjectDocument
        {
            Id = projectId,
            AppVersion = "0.1.0",
            Project = new StartupProject
            {
                Id = projectId,
                Name = kind == StartupProjectKind.DemoSystem ? "Demo System" : "Blank Canvas",
                Kind = kind,
                CreatedAtUtc = createdAtUtc,
                UpdatedAtUtc = createdAtUtc
            },
            Nodes = kind == StartupProjectKind.DemoSystem
                ? CreateDemoNodes(projectId)
                : []
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
                Id = $"{projectId}-inverter-01",
                Kind = "inverter",
                Label = "Inverter I-01",
                X = 360,
                Y = 160
            },
            new ProjectNodePlaceholder
            {
                Id = $"{projectId}-load-01",
                Kind = "load",
                Label = "Essential Load Panel",
                X = 620,
                Y = 220
            }
        ];
    }
}
