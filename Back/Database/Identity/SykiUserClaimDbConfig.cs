using Syki.Back.Domain.Identity;

namespace Syki.Back.Database.Identity;

public class SykiUserClaimDbConfig : IEntityTypeConfiguration<SykiUserClaim>
{
    public void Configure(EntityTypeBuilder<SykiUserClaim> entity)
    {
        entity.ToTable("user_claims", DbSchemas.Syki);

        entity.HasKey(e => e.Id);
    }
}
