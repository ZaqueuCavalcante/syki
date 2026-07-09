using Estud.Back.Domain.Identity;
using Estud.Back.Domain.Students;

namespace Estud.Back.Database.Students;

public class EstudStudentDbConfig : IEntityTypeConfiguration<EstudStudent>
{
    public void Configure(EntityTypeBuilder<EstudStudent> entity)
    {
        entity.ToTable("students", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.HasOne(e => e.User)
            .WithOne()
            .HasPrincipalKey<EstudUser>(u => new { u.InstitutionId, u.Id })
            .HasForeignKey<EstudStudent>(e => new { e.InstitutionId, e.UserId });

        entity.Property(x => x.YieldCoefficient).HasPrecision(4, 2);

        entity.HasIndex(s => s.EnrollmentCode).IsUnique();
    }
}
