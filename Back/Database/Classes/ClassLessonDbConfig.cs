using Estud.Back.Domain.Classes;

namespace Estud.Back.Database.Classes;

public class ClassLessonDbConfig : IEntityTypeConfiguration<ClassLesson>
{
    public void Configure(EntityTypeBuilder<ClassLesson> entity)
    {
        entity.ToTable("class_lessons", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.HasMany(e => e.Attendances)
            .WithOne()
            .HasForeignKey(a => a.LessonId);
    }
}
