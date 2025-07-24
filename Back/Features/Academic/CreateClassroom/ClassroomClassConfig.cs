using Syki.Back.Features.Academic.CreateClass;

namespace Syki.Back.Features.Academic.CreateClassroom;

public class ClassroomClassConfig : IEntityTypeConfiguration<ClassroomClass>
{
    public void Configure(EntityTypeBuilder<ClassroomClass> classroomClass)
    {
        classroomClass.ToTable("classrooms__classes");

        classroomClass.HasKey(x => new { x.ClassroomId, x.ClassId });

        classroomClass.HasOne<Classroom>()
            .WithMany()
            .HasForeignKey(x => x.ClassroomId);

        classroomClass.HasOne<Class>()
            .WithMany()
            .HasForeignKey(x => x.ClassId);
    }
}
