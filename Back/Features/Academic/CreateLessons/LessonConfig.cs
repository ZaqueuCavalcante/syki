namespace Syki.Back.Features.Academic.CreateLessons;

public class LessonConfig : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> lesson)
    {
        lesson.ToTable("lessons");

        lesson.HasKey(l => l.Id);
        lesson.Property(l => l.Id).ValueGeneratedNever();

        lesson.HasMany(l => l.Attendances)
            .WithOne()
            .HasForeignKey(a => a.LessonId);
    }
}
