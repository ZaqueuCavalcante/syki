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
    }
}
