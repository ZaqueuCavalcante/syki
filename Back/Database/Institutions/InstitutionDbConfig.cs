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

        entity.HasMany(e => e.Classes)
            .WithOne()
            .HasForeignKey(c => c.InstitutionId);

        entity.HasMany(e => e.Classrooms)
            .WithOne()
            .HasForeignKey(c => c.InstitutionId);

        entity.HasMany(e => e.Commands)
            .WithOne(c => c.Institution)
            .HasForeignKey(c => c.InstitutionId);

        entity.HasMany(e => e.CommandBatches)
            .WithOne()
            .HasForeignKey(cb => cb.InstitutionId);

        entity.HasMany(e => e.CourseCurriculums)
            .WithOne()
            .HasForeignKey(cc => cc.InstitutionId);

        entity.HasMany(e => e.CourseOfferings)
            .WithOne()
            .HasForeignKey(co => co.InstitutionId);

        entity.HasMany(e => e.Courses)
            .WithOne()
            .HasForeignKey(c => c.InstitutionId);

        entity.HasMany(e => e.Disciplines)
            .WithOne()
            .HasForeignKey(d => d.InstitutionId);

        entity.HasMany(e => e.ResetPasswordTokens)
            .WithOne()
            .HasForeignKey(rpt => rpt.InstitutionId);

        entity.HasMany(e => e.SsoConfigurations)
            .WithOne()
            .HasForeignKey(sc => sc.InstitutionId);

        entity.HasMany(e => e.Users)
            .WithOne(u => u.Institution)
            .HasForeignKey(u => u.InstitutionId);

        entity.HasMany(e => e.Notifications)
            .WithOne()
            .HasForeignKey(n => n.InstitutionId);

        entity.HasMany(e => e.AcademicPeriods)
            .WithOne()
            .HasForeignKey(ap => ap.InstitutionId);

        entity.HasMany(e => e.EnrollmentPeriods)
            .WithOne()
            .HasForeignKey(ep => ep.InstitutionId);

        entity.HasMany(e => e.Students)
            .WithOne()
            .HasForeignKey(s => s.InstitutionId);

        entity.HasMany(e => e.Teachers)
            .WithOne()
            .HasForeignKey(t => t.InstitutionId);

        entity.HasMany(e => e.WebhookSubscriptions)
            .WithOne()
            .HasForeignKey(w => w.InstitutionId);

        entity.HasMany(e => e.WebhookCalls)
            .WithOne()
            .HasForeignKey(w => w.InstitutionId);
    }
}
