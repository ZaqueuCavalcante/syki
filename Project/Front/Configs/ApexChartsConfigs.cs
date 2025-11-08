using ApexCharts;

namespace Exato.Front.Configs;

public static class ApexChartsConfigs
{
    public static void AddApexChartsConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddApexCharts();
    }
}
