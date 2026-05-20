using Syki.Back.Domain.Teachers;

namespace Syki.Back.Database.Teachers;

public class TeacherDisciplineDbConfig : IEntityTypeConfiguration<TeacherDiscipline>
{
    public void Configure(EntityTypeBuilder<TeacherDiscipline> entity)
    {
        entity.ToTable("teachers_disciplines", DbSchemas.Syki);

        entity.HasKey(x => new { x.TeacherId, x.DisciplineId });
    }
}
