using SolarSandBox.UI.Domain;

namespace SolarSandBox.UI.Services;

public interface IConnectionValidationService
{
    ConnectionValidationResult Validate(
        ElectricalConnection candidate,
        IReadOnlyCollection<EquipmentDefinition> equipmentDefinitions,
        IReadOnlyCollection<EquipmentInstance> equipmentInstances,
        IReadOnlyCollection<ElectricalConnection> existingConnections);
}
