namespace Syki.Front.Configs;

public static class SellerServicesConfigs
{
    public static void AddSellerServicesConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<GetSellerCourseOfferingsClient>();
    }
}
