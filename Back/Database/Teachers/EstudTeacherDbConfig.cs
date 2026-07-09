using Estud.Back.Domain.Campi;
using Estud.Back.Domain.Identity;
using Estud.Back.Domain.Teachers;
using Estud.Back.Domain.Disciplines;

namespace Estud.Back.Database.Teachers;

public class EstudTeacherDbConfig : IEntityTypeConfiguration<EstudTeacher>
{
    public void Configure(EntityTypeBuilder<EstudTeacher> entity)
    {
        entity.ToTable("teachers", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.HasOne(e => e.User)
            .WithOne()
            .HasPrincipalKey<EstudUser>(u => new { u.InstitutionId, u.Id })
            .HasForeignKey<EstudTeacher>(e => new { e.InstitutionId, e.UserId });

        entity.HasMany(c => c.Campi)
            .WithMany()
            .UsingEntity<TeacherCampus>(
                right => right.HasOne<Campus>().WithMany().HasForeignKey(tc => tc.CampusId),
                left => left.HasOne<EstudTeacher>().WithMany().HasForeignKey(tc => tc.TeacherId)
            );

        entity.HasMany(e => e.Disciplines)
            .WithMany()
            .UsingEntity<TeacherDiscipline>(
                right => right.HasOne<Discipline>().WithMany().HasForeignKey(td => td.DisciplineId),
                left => left.HasOne<EstudTeacher>().WithMany().HasForeignKey(td => td.TeacherId)
            );
    }
}
