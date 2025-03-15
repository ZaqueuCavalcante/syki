using Syki.Back.Features.Academic.CreateClass;

namespace Syki.Back.Features.Teacher.CreateClassActivity;

public class ClassActivityConfig : IEntityTypeConfiguration<ClassActivity>
{
    public void Configure(EntityTypeBuilder<ClassActivity> classActivity)
    {
        classActivity.ToTable("class_activities");

        classActivity.HasKey(t => t.Id);
        classActivity.Property(t => t.Id).ValueGeneratedNever();

        classActivity.HasOne<Class>()
            .WithMany()
            .HasForeignKey(t => t.ClassId);

        classActivity.HasOne<Lesson>()
            .WithMany()
            .HasForeignKey(t => t.LessonId);

        classActivity.Property(x => x.Value).HasPrecision(4, 2);
    }
}
