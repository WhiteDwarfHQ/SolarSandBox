using Microsoft.JSInterop;
using SolarSandBox.UI.Models;

namespace SolarSandBox.UI.Services;

public sealed class StartupProjectRepository(IJSRuntime jsRuntime) : IStartupProjectRepository
{
    public async Task InitializeAsync()
    {
        await jsRuntime.InvokeVoidAsync("solarSandboxStartupStorage.initialize");
    }

    public async Task<StartupProject?> GetLastActiveProjectAsync()
    {
        return await jsRuntime.InvokeAsync<StartupProject?>("solarSandboxStartupStorage.getLastActiveProject");
    }

    public async Task<StartupProject> CreateProjectAsync(StartupProjectKind kind)
    {
        var now = DateTimeOffset.UtcNow;

        var project = new StartupProject
        {
            Id = Guid.NewGuid().ToString("N"),
            Name = kind switch
            {
                StartupProjectKind.DemoSystem => "Demo System",
                _ => "Blank Canvas"
            },
            Kind = kind,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        var savedProject = await jsRuntime.InvokeAsync<StartupProject>("solarSandboxStartupStorage.createOrUpdateProject", project);
        await SetLastActiveProjectAsync(savedProject.Id);

        return savedProject;
    }

    public async Task SetLastActiveProjectAsync(string projectId)
    {
        await jsRuntime.InvokeVoidAsync("solarSandboxStartupStorage.setLastActiveProject", projectId);
    }
}
