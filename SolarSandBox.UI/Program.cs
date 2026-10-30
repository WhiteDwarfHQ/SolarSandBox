using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SolarSandBox.UI;
using SolarSandBox.UI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddScoped<IProjectBootstrapFactory, ProjectBootstrapFactory>();
builder.Services.AddScoped<IConnectionValidationService, ConnectionValidationService>();
builder.Services.AddScoped<IProjectConnectionApplicationService, ProjectConnectionApplicationService>();
builder.Services.AddScoped<IStartupProjectRepository, StartupProjectRepository>();
builder.Services.AddScoped<ThemeModeService>();

await builder.Build().RunAsync();
