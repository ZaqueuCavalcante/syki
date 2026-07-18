using Estud.Back.Domain.Campi;
using Estud.Back.Domain.Classes;
using Estud.Back.Domain.Teachers;

namespace Estud.Back.Database.Classes;

public class ClassDbConfig : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> entity)
    {
        entity.ToTable("classes", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.HasOne<Campus>()
            .WithMany()
            .HasForeignKey(e => e.CampusId);

        entity.HasMany(e => e.Teachers)
            .WithMany()
            .UsingEntity<ClassTeacher>(
                right => right.HasOne<EstudTeacher>().WithMany().HasForeignKey(ct => ct.TeacherId),
                left => left.HasOne<Class>().WithMany().HasForeignKey(ct => ct.ClassId)
            );

        entity.HasOne(e => e.Discipline)
            .WithMany()
            .HasForeignKey(e => e.DisciplineId);

        entity.HasMany(e => e.Schedules)
            .WithOne(s => s.Class)
            .HasForeignKey(s => s.ClassId);

        entity.HasMany(e => e.Lessons)
            .WithOne(l => l.Class)
            .HasForeignKey(l => l.ClassId);

        entity.HasOne(e => e.Period)
            .WithMany()
            .HasPrincipalKey(p => new { p.Id, p.InstitutionId })
            .HasForeignKey(e => new { e.PeriodId, e.InstitutionId });
    }
}
