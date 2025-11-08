using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Features.Cross.CreateExatoUser;

public class ExatoRoleClaimDbConfig : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> entity)
    {
        entity.ToTable("role_claims", "exato");
    }
}
