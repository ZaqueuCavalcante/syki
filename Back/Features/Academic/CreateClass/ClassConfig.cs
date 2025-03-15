using Syki.Back.Features.Student.CreateStudentEnrollment;

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
            .HasForeignKey(s => s.ClassId);

        @class.HasMany(c => c.Lessons)
            .WithOne()
            .HasForeignKey(l => l.ClassId);

        @class.HasMany(c => c.Activities)
            .WithOne()
            .HasForeignKey(a => a.ClassId);

        @class.HasOne(c => c.Period)
            .WithMany()
            .HasForeignKey(c => new { c.PeriodId, c.InstitutionId });

        @class.HasMany(c => c.ExamGrades)
            .WithOne()
            .HasForeignKey(eg => eg.ClassId);

        @class.HasMany(c => c.Students)
            .WithMany()
            .UsingEntity<ClassStudent>();

        @class.Ignore(c => c.FillRatio);
    }
}
