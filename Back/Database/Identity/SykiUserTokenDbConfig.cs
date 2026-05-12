using Syki.Back.Domain.Identity;

namespace Syki.Back.Database.Identity;

public class SykiUserTokenDbConfig : IEntityTypeConfiguration<SykiUserToken>
{
    public void Configure(EntityTypeBuilder<SykiUserToken> entity)
    {
        entity.ToTable("user_tokens", DbSchemas.Syki);

        entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
    }
}
