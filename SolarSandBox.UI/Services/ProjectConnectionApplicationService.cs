using SolarSandBox.UI.Domain;
using SolarSandBox.UI.Models;

namespace SolarSandBox.UI.Services;

public sealed class ProjectConnectionApplicationService(
    IStartupProjectRepository startupProjectRepository,
    IConnectionValidationService connectionValidationService) : IProjectConnectionApplicationService
{
    public async Task<ConnectionValidationResult> TryCreateConnectionAsync(ElectricalConnection candidate)
    {
        var document = await startupProjectRepository.GetActiveProjectDocumentAsync();
        if (document is null)
        {
            return ConnectionValidationResult.Rejected(new ConnectionValidationMessage
            {
                Code = "project-not-found",
                Severity = DiagnosticSeverity.Critical,
                Description = "No active project is available."
            });
        }

        var result = connectionValidationService.Validate(
            candidate,
            document.EquipmentDefinitions,
            document.EquipmentInstances,
            document.ElectricalConnections);

        if (!result.IsAllowed)
        {
            return result;
        }

        var persistedConnection = new ElectricalConnection
        {
            Id = string.IsNullOrWhiteSpace(candidate.Id) ? Guid.NewGuid().ToString("N") : candidate.Id,
            From = candidate.From,
            To = candidate.To,
            IsVerified = !result.IsProvisional,
            VerificationNotes = result.IsProvisional
                ? "Connection allowed as provisional/unverified due to unknown specifications."
                : "Connection validated against known typed-port constraints."
        };

        var updatedDocument = CloneWithAdditionalConnection(document, persistedConnection);
        await startupProjectRepository.SaveProjectDocumentAsync(updatedDocument);

        return result;
    }

    private static SolarProjectDocument CloneWithAdditionalConnection(SolarProjectDocument source, ElectricalConnection connection)
    {
        return new SolarProjectDocument
        {
            Id = source.Id,
            SchemaVersion = source.SchemaVersion,
            AppVersion = source.AppVersion,
            Project = source.Project,
            Nodes = [.. source.Nodes],
            EquipmentDefinitions = [.. source.EquipmentDefinitions],
            EquipmentInstances = [.. source.EquipmentInstances],
            ElectricalConnections = [.. source.ElectricalConnections, connection]
        };
    }
}
