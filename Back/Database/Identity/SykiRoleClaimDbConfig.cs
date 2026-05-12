using Syki.Back.Domain.Identity;

namespace Syki.Back.Database.Identity;

public class SykiRoleClaimDbConfig : IEntityTypeConfiguration<SykiRoleClaim>
{
    public void Configure(EntityTypeBuilder<SykiRoleClaim> entity)
    {
        entity.ToTable("role_claims", DbSchemas.Syki);

        entity.HasKey(e => e.Id);
    }
}
