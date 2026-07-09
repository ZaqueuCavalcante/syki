using Estud.Back.Domain.Classes;
using Estud.Back.Domain.Classrooms;

namespace Estud.Back.Database.Classrooms;

public class ClassroomClassDbConfig : IEntityTypeConfiguration<ClassroomClass>
{
    public void Configure(EntityTypeBuilder<ClassroomClass> entity)
    {
        entity.ToTable("classrooms__classes", DbSchemas.Estud);

        entity.HasKey(e => new { e.ClassroomId, e.ClassId });

        entity.HasOne<Classroom>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(e => e.ClassroomId);

        entity.HasOne<Class>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(e => e.ClassId);
    }
}
