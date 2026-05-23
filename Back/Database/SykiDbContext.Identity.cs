using Dapper;
using Syki.Back.Cache;
using Syki.Back.Auth.Roles;
using Syki.Back.Domain.Identity;
using Syki.Back.Database.Identity;

namespace Syki.Back.Database;

public partial class SykiDbContext
{
    public DbSet<MagicLink> WebMagicLinks { get; set; }
    public DbSet<SsoConfiguration> WebSsoConfigurations { get; set; }
    public DbSet<SsoAllowedDomain> WebSsoAllowedDomains { get; set; }

    private static void ConfigureIdentity(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MagicLinkDbConfig());

        modelBuilder.ApplyConfiguration(new SykiRoleDbConfig());
        modelBuilder.ApplyConfiguration(new SykiUserDbConfig());
        modelBuilder.ApplyConfiguration(new SykiUserRoleDbConfig());
        modelBuilder.ApplyConfiguration(new SykiRoleClaimDbConfig());
        modelBuilder.ApplyConfiguration(new SykiUserClaimDbConfig());
        modelBuilder.ApplyConfiguration(new SykiUserTokenDbConfig());
        modelBuilder.ApplyConfiguration(new SykiUserLoginDbConfig());

        modelBuilder.ApplyConfiguration(new SsoConfigurationDbConfig());
        modelBuilder.ApplyConfiguration(new SsoAllowedDomainDbConfig());
    }

    public async Task<SykiRole> GetUserRole(int userId, int institutionId)
    {
        var userRole = await UserRoles.Where(x => x.UserId == userId && x.InstitutionId == institutionId).FirstAsync();

        return await Roles.Where(x => x.Id == userRole.RoleId).FirstAsync();
    }

    public async Task<SykiRole> GetDirectorRole()
    {
        return await Cache.GetOrCreateAsync(
            key: $"{CacheKeys.GetDirectorRole}",
            state: this,
            options: new() { Expiration = TimeSpan.FromDays(100) },
            factory: async (state, ct) =>
            {
                return await state.Roles.AsNoTracking()
                    .Where(x => x.OwnerId == null && x.NormalizedName == SykiDefaultRoles.Director.NormalizedName)
                    .FirstAsync(ct);
            }
        );
    }

    public async Task<SykiRole> GetTeacherRole()
    {
        return await Cache.GetOrCreateAsync(
            key: $"{CacheKeys.GetTeacherRole}",
            state: this,
            options: new() { Expiration = TimeSpan.FromDays(100) },
            factory: async (state, ct) =>
            {
                return await state.Roles.AsNoTracking()
                    .Where(x => x.OwnerId == null && x.NormalizedName == SykiDefaultRoles.Teacher.NormalizedName)
                    .FirstAsync(ct);
            }
        );
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
                syki.sso_configurations
            WHERE
                public_id = @PublicId AND is_active = true
            LIMIT 1
        ";

        return await Database.GetDbConnection().QueryFirstOrDefaultAsync<SsoConfiguration?>(sql, new { PublicId = publicId });
    }
}
