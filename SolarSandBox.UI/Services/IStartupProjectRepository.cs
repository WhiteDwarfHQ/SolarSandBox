using SolarSandBox.UI.Models;

namespace SolarSandBox.UI.Services;

public interface IStartupProjectRepository
{
    Task InitializeAsync();

    Task<StartupProject?> GetLastActiveProjectAsync();

    Task<StartupProject> CreateProjectAsync(StartupProjectKind kind);

    Task SetLastActiveProjectAsync(string projectId);
}
