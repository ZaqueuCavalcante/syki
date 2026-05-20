using Syki.Back.Domain.Institutions;

namespace Syki.Back.Database.Institutions;

public class InstitutionDbConfig : IEntityTypeConfiguration<Institution>
{
    public void Configure(EntityTypeBuilder<Institution> entity)
    {
        entity.ToTable("institutions", DbSchemas.Syki);

        entity.HasKey(e => e.Id);

        entity.HasMany(e => e.Campi)
            .WithOne()
            .HasForeignKey(c => c.InstitutionId);

        entity.HasMany(e => e.Courses)
            .WithOne()
            .HasForeignKey(c => c.InstitutionId);

        entity.HasMany(e => e.Users)
            .WithOne(u => u.Institution)
            .HasForeignKey(u => u.InstitutionId);

        entity.HasMany(e => e.Students)
            .WithOne()
            .HasForeignKey(s => s.InstitutionId);

        entity.HasMany(e => e.Teachers)
            .WithOne()
            .HasForeignKey(t => t.InstitutionId);

        entity.HasMany(e => e.Disciplines)
            .WithOne()
            .HasForeignKey(d => d.InstitutionId);

        entity.HasMany(e => e.CourseOfferings)
            .WithOne()
            .HasForeignKey(co => co.InstitutionId);

        entity.HasMany(e => e.AcademicPeriods)
            .WithOne()
            .HasForeignKey(ap => ap.InstitutionId);

        entity.HasMany(e => e.EnrollmentPeriods)
            .WithOne()
            .HasForeignKey(ep => ep.InstitutionId);

        entity.HasMany(e => e.CourseCurriculums)
            .WithOne()
            .HasForeignKey(cc => cc.InstitutionId);
    }
}
