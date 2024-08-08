namespace Syki.Back.Features.Academic.CreateClass;

public class LessonConfig : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> lesson)
    {
        lesson.ToTable("lessons");

        lesson.HasKey(l => l.Id);
        lesson.Property(l => l.Id).ValueGeneratedNever();
    }
}
