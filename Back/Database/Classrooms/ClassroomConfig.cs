using Syki.Back.Domain.Classrooms;

namespace Syki.Back.Database.Classrooms;

public class ClassroomConfig : IEntityTypeConfiguration<Classroom>
{
    public void Configure(EntityTypeBuilder<Classroom> entity)
    {
        entity.ToTable("classrooms", DbSchemas.Syki);

        entity.HasKey(e => e.Id);

        entity.HasOne(e => e.Campus)
            .WithMany()
            .HasForeignKey(e => e.CampusId);

        entity.HasMany(e => e.Schedules)
            .WithOne()
            .HasForeignKey(e => e.ClassroomId);
    }
}
