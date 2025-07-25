using Syki.Back.Features.Academic.CreateCampus;

namespace Syki.Back.Features.Academic.CreateClassroom;

public class ClassroomConfig : IEntityTypeConfiguration<Classroom>
{
    public void Configure(EntityTypeBuilder<Classroom> classroom)
    {
        classroom.ToTable("classrooms");

        classroom.HasKey(c => c.Id);
        classroom.Property(c => c.Id).ValueGeneratedNever();

        classroom.HasOne<Campus>()
            .WithMany()
            .HasForeignKey(c => c.CampusId);
    }
}
