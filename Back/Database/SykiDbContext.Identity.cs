using Syki.Back.Domain.Identity;
using Syki.Back.Database.Identity;

namespace Syki.Back.Database;

public partial class SykiDbContext
{
    public DbSet<MagicLink> WebMagicLinks { get; set; }

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
    }

    public async Task<SykiRole?> GetUserRole(int userId, int institutionId)
    {
        var userRole = await UserRoles.Where(x => x.UserId == userId && x.InstitutionId == institutionId).FirstOrDefaultAsync();

        if (userRole == null) return null;

        return await Roles.Where(x => x.Id == userRole.RoleId).FirstOrDefaultAsync();
    }
}
