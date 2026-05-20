using Syki.Back.Domain.Disciplines;

namespace Syki.Back.Database.Disciplines;

public class DisciplineDbConfig : IEntityTypeConfiguration<Discipline>
{
    public void Configure(EntityTypeBuilder<Discipline> entity)
    {
        entity.ToTable("disciplines", DbSchemas.Syki);

        entity.HasKey(e => e.Id);

        entity.HasIndex(e => e.Code).IsUnique();
    }
}
