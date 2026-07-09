using Estud.Back.Domain.Disciplines;

namespace Estud.Back.Database.Disciplines;

public class DisciplineDbConfig : IEntityTypeConfiguration<Discipline>
{
    public void Configure(EntityTypeBuilder<Discipline> entity)
    {
        entity.ToTable("disciplines", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.HasIndex(e => e.Code).IsUnique();
    }
}
