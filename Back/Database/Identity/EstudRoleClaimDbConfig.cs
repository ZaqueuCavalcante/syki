using Estud.Back.Domain.Identity;

namespace Estud.Back.Database.Identity;

public class EstudRoleClaimDbConfig : IEntityTypeConfiguration<EstudRoleClaim>
{
    public void Configure(EntityTypeBuilder<EstudRoleClaim> entity)
    {
        entity.ToTable("role_claims", DbSchemas.Estud);

        entity.HasKey(e => e.Id);
    }
}
