using Syki.Back.Features.Academic.CreateStudent;
using Syki.Back.Features.Academic.CreateAcademicPeriod;
using Syki.Back.Features.Student.CreateStudentEnrollment;

namespace Syki.Back.Features.Academic.CreateClass;

public class ClassConfig : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> @class)
    {
        @class.ToTable("classes");

        @class.HasKey(t => t.Id);
        @class.Property(t => t.Id).ValueGeneratedNever();

        @class.HasOne(t => t.Teacher)
            .WithMany()
            .HasForeignKey(t => t.TeacherId);

        @class.HasOne(t => t.Discipline)
            .WithMany()
            .HasForeignKey(t => t.DisciplineId);

        @class.HasMany(t => t.Schedules)
            .WithOne()
            .HasForeignKey(h => h.ClassId);

        @class.HasOne<AcademicPeriod>()
            .WithMany()
            .HasForeignKey(t => new { t.Period, t.InstitutionId });
    }
}
