using System.Text.Json;
using Microsoft.JSInterop;
using SolarSandBox.UI.Models;

namespace SolarSandBox.UI.Services;

public sealed class StartupProjectRepository(IJSRuntime jsRuntime, IProjectBootstrapFactory projectBootstrapFactory) : IStartupProjectRepository
{
    private static readonly JsonSerializerOptions ExportSerializerOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task InitializeAsync()
    {
        await jsRuntime.InvokeVoidAsync("solarSandboxStartupStorage.initialize");
    }

    public async Task<StartupProject?> GetLastActiveProjectAsync()
    {
        var document = await jsRuntime.InvokeAsync<SolarProjectDocument?>("solarSandboxStartupStorage.getLastActiveProjectDocument");
        return document?.Project;
    }

    public async Task<StartupProject> CreateProjectAsync(StartupProjectKind kind)
    {
        var document = projectBootstrapFactory.CreateDocument(kind, DateTimeOffset.UtcNow);

        await jsRuntime.InvokeVoidAsync("solarSandboxStartupStorage.createOrUpdateProject", document);
        await SetLastActiveProjectAsync(document.Project.Id);

        return document.Project;
    }

    public async Task SetLastActiveProjectAsync(string projectId)
    {
        await jsRuntime.InvokeVoidAsync("solarSandboxStartupStorage.setLastActiveProject", projectId);
    }

    public async Task<string?> ExportActiveProjectJsonAsync()
    {
        var document = await jsRuntime.InvokeAsync<SolarProjectDocument?>("solarSandboxStartupStorage.getLastActiveProjectDocument");
        if (document is null)
        {
            return null;
        }

        return JsonSerializer.Serialize(document, ExportSerializerOptions);
    }
}
