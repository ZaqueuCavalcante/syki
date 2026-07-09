using Estud.Back.Domain.Identity;

namespace Estud.Back.Database.Identity;

public class EstudUserTokenDbConfig : IEntityTypeConfiguration<EstudUserToken>
{
    public void Configure(EntityTypeBuilder<EstudUserToken> entity)
    {
        entity.ToTable("user_tokens", DbSchemas.Estud);

        entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
    }
}
