namespace Syki.Back.Features.Student.CreateClassActivityWork;

public class ClassActivityWorkConfig : IEntityTypeConfiguration<ClassActivityWork>
{
    public void Configure(EntityTypeBuilder<ClassActivityWork> classActivityWork)
    {
        classActivityWork.ToTable("class_activity_works");

        classActivityWork.HasKey(x => x.Id);
        classActivityWork.Property(x => x.Id).ValueGeneratedNever();

        classActivityWork.HasOne(x => x.SykiStudent)
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.SykiStudentId);
    }
}
