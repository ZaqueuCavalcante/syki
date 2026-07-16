using Estud.Back.Domain.Parents;

namespace Estud.Back.Database.Parents;

public class ParentStudentDbConfig : IEntityTypeConfiguration<ParentStudent>
{
    public void Configure(EntityTypeBuilder<ParentStudent> entity)
    {
        entity.ToTable("parent_students", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.HasOne(e => e.Parent)
            .WithMany()
            .HasForeignKey(e => e.ParentId);

        entity.HasOne(e => e.Student)
            .WithMany()
            .HasForeignKey(e => e.StudentId);

        entity.HasIndex(e => new { e.ParentId, e.StudentId }).IsUnique();
    }
}
