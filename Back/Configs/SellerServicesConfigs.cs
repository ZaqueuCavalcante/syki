using Syki.Back.Features.Seller.GetSellerCourseOfferings;

namespace Syki.Back.Configs;

public static class SellerServicesConfigs
{
    public static void AddSellerServicesConfigs(this IServiceCollection services)
    {
        services.AddScoped<GetSellerCourseOfferingsService>();
    }
}
