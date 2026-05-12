namespace Syki.Back.Features.Academic.CreateClass;

public class ClassLessonConfig : IEntityTypeConfiguration<ClassLesson>
{
    public void Configure(EntityTypeBuilder<ClassLesson> lesson)
    {
        lesson.ToTable("class_lessons");

        lesson.HasKey(l => l.Id);
        lesson.Property(l => l.Id).ValueGeneratedNever();

        lesson.HasMany(l => l.Attendances)
            .WithOne()
            .HasForeignKey(a => a.LessonId);
    }
}
