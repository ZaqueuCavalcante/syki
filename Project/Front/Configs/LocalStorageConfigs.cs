namespace Exato.Front.Configs;

public static class LocalStorageConfigs
{
    public static void AddLocalStorageConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddLocalStorageServices();
    }
}
