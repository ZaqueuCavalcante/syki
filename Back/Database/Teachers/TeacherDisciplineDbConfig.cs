using Estud.Back.Domain.Teachers;

namespace Estud.Back.Database.Teachers;

public class TeacherDisciplineDbConfig : IEntityTypeConfiguration<TeacherDiscipline>
{
    public void Configure(EntityTypeBuilder<TeacherDiscipline> entity)
    {
        entity.ToTable("teachers_disciplines", DbSchemas.Estud);

        entity.HasKey(x => new { x.TeacherId, x.DisciplineId });
    }
}
