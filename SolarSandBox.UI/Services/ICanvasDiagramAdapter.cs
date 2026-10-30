using Blazor.Diagrams;
using SolarSandBox.UI.Models;

namespace SolarSandBox.UI.Services;

public interface ICanvasDiagramAdapter
{
    CanvasDiagramBuildResult Build(SolarProjectDocument document);
}
