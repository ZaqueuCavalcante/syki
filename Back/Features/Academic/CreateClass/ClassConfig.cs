using Syki.Back.Features.Academic.CreateAcademicPeriod;

namespace Syki.Back.Features.Academic.CreateClass;

public class ClassConfig : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> @class)
    {
        @class.ToTable("classes");

        @class.HasKey(c => c.Id);
        @class.Property(c => c.Id).ValueGeneratedNever();

        @class.HasOne(c => c.Teacher)
            .WithMany()
            .HasForeignKey(c => c.TeacherId);

        @class.HasOne(c => c.Discipline)
            .WithMany()
            .HasForeignKey(c => c.DisciplineId);

        @class.HasMany(c => c.Schedules)
            .WithOne()
            .HasForeignKey(h => h.ClassId);

        @class.HasOne<AcademicPeriod>()
            .WithMany()
            .HasForeignKey(c => new { c.Period, c.InstitutionId });

        @class.HasMany(c => c.ExamGrades)
            .WithOne()
            .HasForeignKey(eg => eg.ClassId);

        @class.Ignore(c => c.FillRatio);
    }
}
