using Syki.Back.Commands.Domain.Classes;

namespace Syki.Back.Database.Classes;

public class ClassLessonConfig : IEntityTypeConfiguration<ClassLesson>
{
    public void Configure(EntityTypeBuilder<ClassLesson> entity)
    {
        entity.ToTable("class_lessons", DbSchemas.Syki);

        entity.HasKey(e => e.Id);

        entity.HasMany(e => e.Attendances)
            .WithOne()
            .HasForeignKey(a => a.LessonId);
    }
}
