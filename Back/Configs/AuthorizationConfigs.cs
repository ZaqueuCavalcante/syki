namespace Syki.Back.Configs;

public static class AuthorizationConfigs
{
    public static void AddAuthorizationConfigs(this IServiceCollection services)
    {
        services.AddAuthorization(x =>
        {
            x.AddPolicy(TrackApiPolicy.CreateSegmentation, p => p.UserHasFeature(TrackFeature.CanCreateSegmentation));
            
            x.AddPolicy(TrackApiPolicy.GetCampaigns, p => p.UserHasAnyFeature(TrackFeature.CanGetCampaigns, TrackFeature.CanCreateSegmentation));

            x.AddPolicy(TrackApiPolicy.GetDonors, p => p.UserHasAnyFeature(TrackFeature.CanGetFilteredDonors, TrackFeature.CanCreateSegmentation));
        });
    }
}
