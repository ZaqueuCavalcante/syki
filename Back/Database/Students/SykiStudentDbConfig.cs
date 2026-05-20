using Syki.Back.Domain.Identity;
using Syki.Back.Domain.Students;

namespace Syki.Back.Database.Students;

public class SykiStudentDbConfig : IEntityTypeConfiguration<SykiStudent>
{
    public void Configure(EntityTypeBuilder<SykiStudent> entity)
    {
        entity.ToTable("students", DbSchemas.Syki);

        entity.HasKey(e => e.Id);

        entity.HasOne(e => e.User)
            .WithOne()
            .HasPrincipalKey<SykiUser>(u => new { u.InstitutionId, u.Id })
            .HasForeignKey<SykiStudent>(e => new { e.InstitutionId, e.Id });

        entity.Property(x => x.YieldCoefficient).HasPrecision(4, 2);

        entity.HasIndex(s => s.EnrollmentCode).IsUnique();
    }
}
