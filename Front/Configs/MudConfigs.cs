using MudBlazor;
using MudBlazor.Services;

namespace Syki.Front.Configs;

public static class MudConfigs
{
    public static void AddMudConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.NewestOnTop = true;
            config.SnackbarConfiguration.ShowCloseIcon = false;
            config.SnackbarConfiguration.PreventDuplicates = true;
            config.SnackbarConfiguration.VisibleStateDuration = 1_500;
            config.SnackbarConfiguration.ShowTransitionDuration = 200;
            config.SnackbarConfiguration.HideTransitionDuration = 200;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
        });
    }
}
