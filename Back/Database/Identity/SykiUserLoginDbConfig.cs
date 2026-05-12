using Syki.Back.Domain.Identity;

namespace Syki.Back.Database.Identity;

public class SykiUserLoginDbConfig : IEntityTypeConfiguration<SykiUserLogin>
{
    public void Configure(EntityTypeBuilder<SykiUserLogin> entity)
    {
        entity.ToTable("user_logins", DbSchemas.Syki);

        entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
    }
}
