using SolarSandBox.UI.Domain;

namespace SolarSandBox.UI.Services;

public interface IProjectConnectionApplicationService
{
    Task<ConnectionValidationResult> TryCreateConnectionAsync(ElectricalConnection candidate);
}
