namespace Syki.Back.Features.Teacher.CreateClassActivity;

public class ClassActivityConfig : IEntityTypeConfiguration<ClassActivity>
{
    public void Configure(EntityTypeBuilder<ClassActivity> classActivity)
    {
        classActivity.ToTable("class_activities");

        classActivity.HasKey(t => t.Id);
        classActivity.Property(t => t.Id).ValueGeneratedNever();

        classActivity.HasMany(c => c.Works)
            .WithOne()
            .HasForeignKey(l => l.ClassActivityId);
    }
}
