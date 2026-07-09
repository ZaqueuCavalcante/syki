using Estud.Back.Domain.Classes;
using Estud.Back.Domain.Students;

namespace Estud.Back.Database.Classes;

public class ClassStudentDbConfig : IEntityTypeConfiguration<ClassStudent>
{
    public void Configure(EntityTypeBuilder<ClassStudent> entity)
    {
        entity.ToTable("classes__students", DbSchemas.Estud);

        entity.HasKey(t => new { t.ClassId, t.StudentId });

        entity.HasOne<Class>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.ClassId);

        entity.HasOne<EstudStudent>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.StudentId);
    }
}
