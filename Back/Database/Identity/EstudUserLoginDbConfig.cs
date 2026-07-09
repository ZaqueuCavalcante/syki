using Estud.Back.Domain.Identity;

namespace Estud.Back.Database.Identity;

public class EstudUserLoginDbConfig : IEntityTypeConfiguration<EstudUserLogin>
{
    public void Configure(EntityTypeBuilder<EstudUserLogin> entity)
    {
        entity.ToTable("user_logins", DbSchemas.Estud);

        entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
    }
}
