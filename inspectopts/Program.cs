using System.Reflection;
using Blazor.Diagrams.Options;

var options = new BlazorDiagramOptions();
Console.WriteLine($"AllowPanning={options.AllowPanning}");
Console.WriteLine($"GridSize={options.GridSize}");

var link = options.Links;
Console.WriteLine($"Links.DefaultRouter={link.DefaultRouter?.GetType().Name}");
Console.WriteLine($"Links.EnableSnapping={link.EnableSnapping}");

foreach (var prop in typeof(BlazorDiagramLinkOptions).GetProperties(BindingFlags.Public | BindingFlags.Instance))
{
    Console.WriteLine($"LINK_PROP {prop.Name}={prop.GetValue(link) ?? "<null>"}");
}

foreach (var prop in typeof(BlazorDiagramOptions).GetProperties(BindingFlags.Public | BindingFlags.Instance))
{
    Console.WriteLine($"OPT_PROP {prop.Name}={prop.GetValue(options) ?? "<null>"}");
}
