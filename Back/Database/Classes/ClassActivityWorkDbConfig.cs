using Estud.Back.Domain.Classes;

namespace Estud.Back.Database.Classes;

public class ClassActivityWorkDbConfig : IEntityTypeConfiguration<ClassActivityWork>
{
    public void Configure(EntityTypeBuilder<ClassActivityWork> entity)
    {
        entity.ToTable("class_activity_works", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.HasOne(e => e.Student)
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(e => e.StudentId);
    }
}
