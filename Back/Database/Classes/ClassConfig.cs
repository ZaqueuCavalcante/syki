using Syki.Back.Domain.Campi;
using Syki.Back.Commands.Domain.Classes;

namespace Syki.Back.Database.Classes;

public class ClassConfig : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> entity)
    {
        entity.ToTable("classes", DbSchemas.Syki);

        entity.HasKey(e => e.Id);

        entity.HasOne<Campus>()
            .WithMany()
            .HasForeignKey(e => e.CampusId);

        entity.HasOne(e => e.Teacher)
            .WithMany()
            .HasForeignKey(e => e.TeacherId);

        entity.HasOne(e => e.Discipline)
            .WithMany()
            .HasForeignKey(e => e.DisciplineId);

        entity.HasMany(e => e.Schedules)
            .WithOne()
            .HasForeignKey(s => s.ClassId);

        entity.HasMany(e => e.Lessons)
            .WithOne()
            .HasForeignKey(l => l.ClassId);

        entity.HasOne(e => e.Period)
            .WithMany()
            .HasForeignKey(e => new { e.PeriodId, e.InstitutionId });

        entity.Ignore(e => e.FillRatio);
    }
}
