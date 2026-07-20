using Estud.Back.Domain.Classes;

namespace Estud.Back.Database.Classes;

public class ClassStudentDbConfig : IEntityTypeConfiguration<ClassStudent>
{
    public void Configure(EntityTypeBuilder<ClassStudent> entity)
    {
        entity.ToTable("classes__students", DbSchemas.Estud);

        entity.HasKey(t => new { t.ClassId, t.StudentId });

        entity.HasOne(x => x.Class)
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.ClassId);

        entity.HasOne(x => x.Student)
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.StudentId);
    }
}
