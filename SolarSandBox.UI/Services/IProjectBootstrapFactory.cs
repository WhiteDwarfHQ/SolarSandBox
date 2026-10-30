using SolarSandBox.UI.Models;

namespace SolarSandBox.UI.Services;

public interface IProjectBootstrapFactory
{
    SolarProjectDocument CreateDocument(StartupProjectKind kind, DateTimeOffset createdAtUtc);
}
