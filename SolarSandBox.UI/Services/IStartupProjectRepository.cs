using SolarSandBox.UI.Models;

namespace SolarSandBox.UI.Services;

public interface IStartupProjectRepository
{
    Task InitializeAsync();

    Task<StartupProject?> GetLastActiveProjectAsync();

    Task<StartupProject> CreateProjectAsync(StartupProjectKind kind);

    Task SetLastActiveProjectAsync(string projectId);

    Task<SolarProjectDocument?> GetActiveProjectDocumentAsync();

    Task SaveProjectDocumentAsync(SolarProjectDocument document);

    Task<string?> ExportActiveProjectJsonAsync();
}
