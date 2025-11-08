using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Features.Cross.CreateExatoUser;

public class ExatoUserClaimDbConfig : IEntityTypeConfiguration<IdentityUserClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> entity)
    {
        entity.ToTable("user_claims", "exato");
    }
}
