using Estud.Back.Domain.Identity;

namespace Estud.Back.Database.Identity;

public class EstudUserDbConfig : IEntityTypeConfiguration<EstudUser>
{
    public void Configure(EntityTypeBuilder<EstudUser> entity)
    {
        entity.ToTable("users", DbSchemas.Estud);

        entity.HasKey(e => e.Id);
    }
}
