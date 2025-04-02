using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Configs;

public static class IdentityConfigs
{
    public static void AddIdentityConfigs(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<SykiUser, SykiRole>()
            .AddEntityFrameworkStores<SykiDbContext>()
            .AddDefaultTokenProviders();

        // TODO: Validate this with integration tests (password reset)
        builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromHours(1);
        });

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_@.+";
        });

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;            // The minimum length.
            options.Password.RequireDigit = true;           // Requires a number between 0-9.
            options.Password.RequireLowercase = true;       // Requires a lowercase character.
            options.Password.RequireUppercase = true;       // Requires an uppercase character.
            options.Password.RequiredUniqueChars = 1;       // Requires the minimum number of distinct characters.
            options.Password.RequireNonAlphanumeric = true; // Requires a non-alphanumeric character (@, %, #, !, &, $, ...).
        });

        // TODO: Validate this with integration tests
        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Lockout.AllowedForNewUsers = true;                        // Determines if a new user can be locked out.
            options.Lockout.MaxFailedAccessAttempts = 3;                      // The number of failed access attempts until a user is locked out, if lockout is enabled.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // The amount of time a user is locked out when a lockout occurs.
        });
    }
}
