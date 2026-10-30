using Bunit;
using Microsoft.Extensions.DependencyInjection;
using SolarSandBox.UI.Domain;
using SolarSandBox.UI.Models;
using SolarSandBox.UI.Pages;
using SolarSandBox.UI.Services;

namespace SolarSandBox.UI.Tests;

public sealed class HomeComponentTests : TestContext
{
    [Fact]
    public void Home_WhenNoActiveProject_ShowsProjectSelectionModal()
    {
        var repository = new FakeStartupProjectRepository
        {
            LastActiveProject = null
        };

        Services.AddSingleton<IStartupProjectRepository>(repository);
        Services.AddSingleton<IProjectConnectionApplicationService, FakeProjectConnectionApplicationService>();
        Services.AddSingleton<IProjectBootstrapFactory, FakeProjectBootstrapFactory>();
        Services.AddSingleton<ICanvasDiagramAdapter, CanvasDiagramAdapter>();

        var cut = RenderComponent<Home>();

        cut.WaitForAssertion(() =>
        {
            Assert.NotNull(cut.Find(".project-modal"));
            Assert.Contains("Blank Canvas", cut.Markup);
            Assert.Contains("Demo System", cut.Markup);
        });
    }

    [Fact]
    public void Home_WhenActiveProject_ExportInvokesBrowserDownload()
    {
        var repository = new FakeStartupProjectRepository
        {
            LastActiveProject = new StartupProject
            {
                Id = "active-01",
                Name = "Blank Canvas",
                Kind = StartupProjectKind.BlankCanvas,
                CreatedAtUtc = DateTimeOffset.UtcNow,
                UpdatedAtUtc = DateTimeOffset.UtcNow
            },
            ExportJson = "{\"schemaVersion\":1}"
        };

        Services.AddSingleton<IStartupProjectRepository>(repository);
        Services.AddSingleton<IProjectConnectionApplicationService, FakeProjectConnectionApplicationService>();
        Services.AddSingleton<IProjectBootstrapFactory, FakeProjectBootstrapFactory>();
        Services.AddSingleton<ICanvasDiagramAdapter, CanvasDiagramAdapter>();
        JSInterop.SetupVoid("solarSandboxApp.downloadTextFile", _ => true);

        var cut = RenderComponent<Home>();

        cut.WaitForAssertion(() => Assert.Contains("Export JSON", cut.Markup));

        cut.FindAll("button")
            .Single(button => button.TextContent.Contains("Export JSON", StringComparison.Ordinal))
            .Click();

        cut.WaitForAssertion(() =>
        {
            Assert.Contains(JSInterop.Invocations, invocation => invocation.Identifier == "solarSandboxApp.downloadTextFile");
        });
    }

    private sealed class FakeStartupProjectRepository : IStartupProjectRepository
    {
        public StartupProject? LastActiveProject { get; set; }

        public string? ExportJson { get; set; }

        public SolarProjectDocument? ActiveDocument { get; set; }

        public Task InitializeAsync() => Task.CompletedTask;

        public Task<StartupProject?> GetLastActiveProjectAsync() => Task.FromResult(LastActiveProject);

        public Task<StartupProject> CreateProjectAsync(StartupProjectKind kind)
        {
            var now = DateTimeOffset.UtcNow;
            LastActiveProject = new StartupProject
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = kind == StartupProjectKind.DemoSystem ? "Demo System" : "Blank Canvas",
                Kind = kind,
                CreatedAtUtc = now,
                UpdatedAtUtc = now
            };

            ActiveDocument = new SolarProjectDocument
            {
                Id = LastActiveProject.Id,
                SchemaVersion = 2,
                AppVersion = "0.1.0",
                Project = LastActiveProject,
                Nodes = [],
                EquipmentDefinitions = [],
                EquipmentInstances = [],
                ElectricalConnections = []
            };

            ExportJson = "{\"schemaVersion\":2}";

            return Task.FromResult(LastActiveProject);
        }

        public Task SetLastActiveProjectAsync(string projectId)
        {
            if (LastActiveProject is not null)
            {
                LastActiveProject = new StartupProject
                {
                    Id = projectId,
                    Name = LastActiveProject.Name,
                    Kind = LastActiveProject.Kind,
                    CreatedAtUtc = LastActiveProject.CreatedAtUtc,
                    UpdatedAtUtc = DateTimeOffset.UtcNow
                };
            }

            return Task.CompletedTask;
        }

        public Task<SolarProjectDocument?> GetActiveProjectDocumentAsync() => Task.FromResult(ActiveDocument);

        public Task SaveProjectDocumentAsync(SolarProjectDocument document)
        {
            ActiveDocument = document;
            LastActiveProject = document.Project;
            return Task.CompletedTask;
        }

        public Task<string?> ExportActiveProjectJsonAsync() => Task.FromResult(ExportJson);
    }

    private sealed class FakeProjectConnectionApplicationService : IProjectConnectionApplicationService
    {
        public Task<ConnectionValidationResult> TryCreateConnectionAsync(ElectricalConnection candidate)
        {
            return Task.FromResult(ConnectionValidationResult.Rejected(new ConnectionValidationMessage
            {
                Code = "not-implemented-in-test",
                Severity = DiagnosticSeverity.Warning,
                Description = "Connection app service test double."
            }));
        }
    }

    private sealed class FakeProjectBootstrapFactory : IProjectBootstrapFactory
    {
        public SolarProjectDocument CreateDocument(StartupProjectKind kind, DateTimeOffset createdAtUtc)
        {
            var projectId = Guid.NewGuid().ToString("N");
            var project = new StartupProject
            {
                Id = projectId,
                Name = kind == StartupProjectKind.DemoSystem ? "Demo System" : "Blank Canvas",
                Kind = kind,
                CreatedAtUtc = createdAtUtc,
                UpdatedAtUtc = createdAtUtc
            };

            return new SolarProjectDocument
            {
                Id = projectId,
                SchemaVersion = 2,
                AppVersion = "0.1.0",
                Project = project,
                Nodes = [],
                EquipmentDefinitions = [],
                EquipmentInstances = [],
                ElectricalConnections = []
            };
        }
    }
}
