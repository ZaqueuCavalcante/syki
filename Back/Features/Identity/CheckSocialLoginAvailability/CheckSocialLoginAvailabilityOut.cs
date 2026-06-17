namespace Syki.Back.Features.Identity.CheckSocialLoginAvailability;

public class CheckSocialLoginAvailabilityOut : IApiDto<CheckSocialLoginAvailabilityOut>
{
    public bool GoogleEnabled { get; set; }
    public string? GoogleClientId { get; set; }

    public static IEnumerable<(string Name, CheckSocialLoginAvailabilityOut Value)> GetExamples() =>
    [
        ("Google Enabled",
        new CheckSocialLoginAvailabilityOut
        {
            GoogleEnabled = true,
            GoogleClientId = "123456789.apps.googleusercontent.com",
        })
    ];
}
