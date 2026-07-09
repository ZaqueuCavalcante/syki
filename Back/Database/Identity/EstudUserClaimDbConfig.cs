using Estud.Back.Domain.Identity;

namespace Estud.Back.Database.Identity;

public class EstudUserClaimDbConfig : IEntityTypeConfiguration<EstudUserClaim>
{
    public void Configure(EntityTypeBuilder<EstudUserClaim> entity)
    {
        entity.ToTable("user_claims", DbSchemas.Estud);

        entity.HasKey(e => e.Id);
    }
}
