using Estud.Back.Domain.Classrooms;

namespace Estud.Back.Database.Classrooms;

public class ClassroomDbConfig : IEntityTypeConfiguration<Classroom>
{
    public void Configure(EntityTypeBuilder<Classroom> entity)
    {
        entity.ToTable("classrooms", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.HasOne(e => e.Campus)
            .WithMany()
            .HasForeignKey(e => e.CampusId);

        entity.HasMany(e => e.Schedules)
            .WithOne()
            .HasForeignKey(e => e.ClassroomId);
    }
}
