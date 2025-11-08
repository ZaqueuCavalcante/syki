using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Features.Cross.CreateExatoUser;

public class ExatoUserTokenDbConfig : IEntityTypeConfiguration<IdentityUserToken<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> entity)
    {
        entity.ToTable("user_tokens", "exato");
    }
}
