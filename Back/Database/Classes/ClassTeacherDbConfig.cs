using Estud.Back.Domain.Classes;

namespace Estud.Back.Database.Classes;

public class ClassTeacherDbConfig : IEntityTypeConfiguration<ClassTeacher>
{
    public void Configure(EntityTypeBuilder<ClassTeacher> entity)
    {
        entity.ToTable("classes__teachers", DbSchemas.Estud);

        entity.HasKey(t => new { t.ClassId, t.TeacherId });
    }
}
