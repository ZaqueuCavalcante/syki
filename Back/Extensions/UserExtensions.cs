using Newtonsoft.Json;
using System.Security.Claims;
using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Extensions;

public enum TrackFeature
{
    CanCreateSegmentation = 0,
    CanGetCampaigns = 1,
    CanGetFilteredDonors = 2,
}

public static class TrackApiPolicy
{
    public const string CreateSegmentation = nameof(CreateSegmentation);
    public const string GetCampaigns = nameof(GetCampaigns);
    public const string GetDonors = nameof(GetDonors);
}

// public static class TrackFrontPolicy
// {
//     public static readonly string CreateSegmentation = nameof(CreateSegmentation);
//     public static readonly string GetCampaigns = nameof(GetCampaigns);
//     public static readonly string GetFilteredDonors = nameof(GetFilteredDonors);
// }

public static class UserExtensions
{
    public static bool HasFeature(this ClaimsPrincipal user, TrackFeature feature)
    {
        if (user == null || !user.Identity.IsAuthenticated) return false;

        var userFeatures = JsonConvert.DeserializeObject<int[]>(user.FindFirstValue("features"));
        return userFeatures.Contains((int)feature);
    }

    public static bool HasAnyFeature(this ClaimsPrincipal user, params TrackFeature[] features)
    {
        if (user == null || !user.Identity.IsAuthenticated) return false;

        var userFeatures = JsonConvert.DeserializeObject<int[]>(user.FindFirstValue("features"));
        foreach (var feature in features)
        {
            if (userFeatures.Contains((int)feature)) return true;
        }
        return false;
    }

    public static AuthorizationPolicyBuilder UserHasFeature(this AuthorizationPolicyBuilder builder, TrackFeature feature)
    {
        builder.RequireAuthenticatedUser().RequireAssertion(ctx => ctx.User.HasFeature(feature));
        return builder;
    }

    public static AuthorizationPolicyBuilder UserHasAnyFeature(this AuthorizationPolicyBuilder builder, params TrackFeature[] features)
    {
        builder.RequireAuthenticatedUser().RequireAssertion(ctx => ctx.User.HasAnyFeature(features));
        return builder;
    }










    public static Guid InstitutionId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue("institution")!);
    }

    public static Guid Id(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue("sub")!);
    }

    public static Guid GetCourseCurriculumId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue("CourseCurriculumId")!);
    }

    public static async Task<bool> IsOnlyInRole(this UserManager<SykiUser> userManager, SykiUser user, UserRole role)
    {
        var adm = await userManager.IsInRoleAsync(user!, UserRole.Adm.ToString());
        var student = await userManager.IsInRoleAsync(user!, UserRole.Student.ToString());
        var teacher = await userManager.IsInRoleAsync(user!, UserRole.Teacher.ToString());
        var academic = await userManager.IsInRoleAsync(user!, UserRole.Academic.ToString());
        var seller = await userManager.IsInRoleAsync(user!, UserRole.Seller.ToString());

        return role switch
        {
            UserRole.Academic => academic && !(adm || student || teacher || seller),
            UserRole.Student => student && !(adm || academic || teacher || seller),
            UserRole.Teacher => teacher && !(adm || student || academic || seller),
            UserRole.Adm => adm && !(student || teacher || academic || seller),
            UserRole.Seller => seller && !(adm || student || teacher || academic || seller),
            _ => false
        };
    }

    public static bool IsAuditable(this PathString path)
    {
        return
            path != "/login" &&
            path != "/login/mfa" &&
            path != "/reset-password" &&
            path != "/users" &&
            path != "/reset-password-token";
    }
}
