using System.Text.Json;
using SolarSandBox.UI.Domain;
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
        var document = await LoadNormalizedActiveDocumentAsync();
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

    public async Task<SolarProjectDocument?> GetActiveProjectDocumentAsync()
    {
        return await LoadNormalizedActiveDocumentAsync();
    }

    public async Task SaveProjectDocumentAsync(SolarProjectDocument document)
    {
        await jsRuntime.InvokeVoidAsync("solarSandboxStartupStorage.createOrUpdateProject", document);
        await SetLastActiveProjectAsync(document.Id);
    }

    public async Task<string?> ExportActiveProjectJsonAsync()
    {
        var document = await LoadNormalizedActiveDocumentAsync();
        if (document is null)
        {
            return null;
        }

        return JsonSerializer.Serialize(document, ExportSerializerOptions);
    }

    private async Task<SolarProjectDocument?> LoadNormalizedActiveDocumentAsync()
    {
        var document = await jsRuntime.InvokeAsync<SolarProjectDocument?>("solarSandboxStartupStorage.getLastActiveProjectDocument");
        if (document is null)
        {
            return null;
        }

        if (document.SchemaVersion >= 2
            && document.EquipmentDefinitions is not null
            && document.EquipmentInstances is not null
            && document.ElectricalConnections is not null)
        {
            return document;
        }

        var normalized = new SolarProjectDocument
        {
            Id = document.Id,
            SchemaVersion = 2,
            AppVersion = document.AppVersion,
            Project = document.Project,
            Nodes = document.Nodes ?? [],
            EquipmentDefinitions = document.EquipmentDefinitions ?? [],
            EquipmentInstances = document.EquipmentInstances ?? [],
            ElectricalConnections = document.ElectricalConnections ?? []
        };

        await jsRuntime.InvokeVoidAsync("solarSandboxStartupStorage.createOrUpdateProject", normalized);

        return normalized;
    }
}
