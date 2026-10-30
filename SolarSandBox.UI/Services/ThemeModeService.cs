using Microsoft.JSInterop;

namespace SolarSandBox.UI.Services;

public sealed class ThemeModeService(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private const string ThemeModeDark = "dark";
    private const string ThemeModeLight = "light";
    private const string ThemeModeSystem = "system";

    private DotNetObjectReference<ThemeModeService>? _selfReference;
    private string _themeMode = ThemeModeDark;

    public bool IsDarkMode { get; private set; } = true;

    public event Action? ThemeChanged;

    public async Task InitializeAsync()
    {
        var storedThemeMode = await jsRuntime.InvokeAsync<string?>("solarSandboxApp.getThemePreference");
        _themeMode = NormalizeThemeMode(storedThemeMode);

        IsDarkMode = _themeMode switch
        {
            ThemeModeLight => false,
            ThemeModeSystem => await jsRuntime.InvokeAsync<bool>("solarSandboxApp.getPrefersDarkMode"),
            _ => true
        };

        if (string.IsNullOrWhiteSpace(storedThemeMode))
        {
            await jsRuntime.InvokeVoidAsync("solarSandboxApp.setThemePreference", ThemeModeDark);
        }

        _selfReference ??= DotNetObjectReference.Create(this);
        await jsRuntime.InvokeVoidAsync("solarSandboxApp.subscribeToThemePreference", _selfReference);

        ThemeChanged?.Invoke();
    }

    public async Task ToggleThemeAsync()
    {
        IsDarkMode = !IsDarkMode;
        _themeMode = IsDarkMode ? ThemeModeDark : ThemeModeLight;

        await jsRuntime.InvokeVoidAsync("solarSandboxApp.setThemePreference", _themeMode);
        ThemeChanged?.Invoke();
    }

    [JSInvokable]
    public void OnSystemThemeChanged(bool isDarkMode)
    {
        if (_themeMode != ThemeModeSystem || IsDarkMode == isDarkMode)
        {
            return;
        }

        IsDarkMode = isDarkMode;
        ThemeChanged?.Invoke();
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            await jsRuntime.InvokeVoidAsync("solarSandboxApp.unsubscribeFromThemePreference");
        }
        catch
        {
            // No-op during teardown when JS runtime may be unavailable.
        }

        _selfReference?.Dispose();
    }

    private static string NormalizeThemeMode(string? themeMode)
    {
        return themeMode?.Trim().ToLowerInvariant() switch
        {
            ThemeModeLight => ThemeModeLight,
            ThemeModeSystem => ThemeModeSystem,
            _ => ThemeModeDark
        };
    }
}
