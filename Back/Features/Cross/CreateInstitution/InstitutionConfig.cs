namespace Syki.Back.Features.Cross.CreateInstitution;

public class InstitutionConfig : IEntityTypeConfiguration<Institution>
{
    public void Configure(EntityTypeBuilder<Institution> institution)
    {
        institution.ToTable("institutions");

        institution.HasKey(f => f.Id);
        institution.Property(f => f.Id).ValueGeneratedNever();

        institution.HasMany(f => f.AcademicPeriods)
            .WithOne()
            .HasForeignKey(p => p.InstitutionId);

        institution.HasMany(f => f.Campi)
            .WithOne()
            .HasForeignKey(c => c.InstitutionId);

        institution.HasMany(f => f.Courses)
            .WithOne()
            .HasForeignKey(c => c.InstitutionId);

        institution.HasMany(f => f.CourseOfferings)
            .WithOne()
            .HasForeignKey(co => co.InstitutionId);

        institution.HasMany(f => f.CourseCurriculums)
            .WithOne()
            .HasForeignKey(g => g.InstitutionId);

        institution.HasMany(f => f.Disciplines)
            .WithOne()
            .HasForeignKey(d => d.InstitutionId);

        institution.HasMany(f => f.Teachers)
            .WithOne()
            .HasForeignKey(p => p.InstitutionId);

        institution.HasMany(f => f.Students)
            .WithOne()
            .HasForeignKey(a => a.InstitutionId);

        institution.HasMany(i => i.Users)
            .WithOne()
            .HasForeignKey(u => u.InstitutionId);

        institution.HasMany(i => i.Webhooks)
            .WithOne()
            .HasForeignKey(u => u.InstitutionId);

        institution.HasMany(i => i.WebhookCalls)
            .WithOne()
            .HasForeignKey(u => u.InstitutionId);

        institution.HasMany(f => f.Notifications)
            .WithOne()
            .HasForeignKey(n => n.InstitutionId);
    }
}
