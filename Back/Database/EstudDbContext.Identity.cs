using Dapper;
using Estud.Back.Domain.Identity;
using Estud.Back.Database.Identity;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    public DbSet<MagicLink> WebMagicLinks { get; set; }
    public DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; }

    public DbSet<SsoConfiguration> WebSsoConfigurations { get; set; }
    public DbSet<SsoAllowedDomain> WebSsoAllowedDomains { get; set; }

    public DbSet<UserSocialLogin> UserSocialLogins { get; set; }

    private static void ConfigureIdentity(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MagicLinkDbConfig());
        modelBuilder.ApplyConfiguration(new ResetPasswordTokenDbConfig());

        modelBuilder.ApplyConfiguration(new EstudRoleDbConfig());
        modelBuilder.ApplyConfiguration(new EstudUserDbConfig());
        modelBuilder.ApplyConfiguration(new EstudUserRoleDbConfig());
        modelBuilder.ApplyConfiguration(new EstudRoleClaimDbConfig());
        modelBuilder.ApplyConfiguration(new EstudUserClaimDbConfig());
        modelBuilder.ApplyConfiguration(new EstudUserTokenDbConfig());
        modelBuilder.ApplyConfiguration(new EstudUserLoginDbConfig());

        modelBuilder.ApplyConfiguration(new SsoConfigurationDbConfig());
        modelBuilder.ApplyConfiguration(new SsoAllowedDomainDbConfig());

        modelBuilder.ApplyConfiguration(new UserSocialLoginDbConfig());

        modelBuilder.ApplyConfiguration(new DataProtectionKeyDbConfig());
    }

    public async Task<EstudRole> GetUserRole(int userId, int institutionId)
    {
        var userRole = await UserRoles.Where(x => x.UserId == userId && x.InstitutionId == institutionId).FirstAsync();

        return await Roles.Where(x => x.Id == userRole.RoleId).FirstAsync();
    }

    public async Task<SsoConfiguration?> GetActiveSsoConfigForSchemeAsync(Guid publicId)
    {
        const string sql = @"
            SELECT
                id,
                external_id,
                authority,
                client_id,
                client_secret,
                updated_at
            FROM
                estud.sso_configurations
            WHERE
                public_id = @PublicId AND is_active = true
            LIMIT 1
        ";

        return await Database.GetDbConnection().QueryFirstOrDefaultAsync<SsoConfiguration?>(sql, new { PublicId = publicId });
    }

    public async Task<bool> EmailRequiresSsoAsync(string email)
    {
        const string sql = @"
            SELECT
                count(1) > 0
            FROM
                estud.sso_allowed_domains d
            INNER JOIN
                estud.sso_configurations c ON c.id = d.sso_configuration_id
            WHERE
                d.domain = @Domain
                    AND
                c.is_active = true
                    AND
                c.require_sso = true
        ";

        var domain = email.Split('@').Last().ToLowerInvariant();
        return await Database.GetDbConnection().QuerySingleAsync<bool>(sql, new { domain });
    }

    public async Task<string?> GetUserTwoFactorKeyAsync(int userId)
    {
        return await UserTokens.Where(x => x.UserId == userId && x.LoginProvider == "[AspNetUserStore]" && x.Name == "AuthenticatorKey")
            .Select(x => x.Value)
            .FirstOrDefaultAsync();
    }
}
