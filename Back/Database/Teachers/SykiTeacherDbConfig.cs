using Syki.Back.Domain.Campi;
using Syki.Back.Domain.Identity;
using Syki.Back.Domain.Teachers;
using Syki.Back.Domain.Disciplines;

namespace Syki.Back.Database.Teachers;

public class SykiTeacherDbConfig : IEntityTypeConfiguration<SykiTeacher>
{
    public void Configure(EntityTypeBuilder<SykiTeacher> entity)
    {
        entity.ToTable("teachers", DbSchemas.Syki);

        entity.HasKey(e => e.Id);

        entity.HasOne(e => e.User)
            .WithOne()
            .HasPrincipalKey<SykiUser>(u => new { u.InstitutionId, u.Id })
            .HasForeignKey<SykiTeacher>(e => new { e.InstitutionId, e.UserId });

        entity.HasMany(c => c.Campi)
            .WithMany()
            .UsingEntity<TeacherCampus>(
                right => right.HasOne<Campus>().WithMany().HasForeignKey(tc => tc.CampusId),
                left => left.HasOne<SykiTeacher>().WithMany().HasForeignKey(tc => tc.TeacherId)
            );

        entity.HasMany(e => e.Disciplines)
            .WithMany()
            .UsingEntity<TeacherDiscipline>(
                right => right.HasOne<Discipline>().WithMany().HasForeignKey(td => td.DisciplineId),
                left => left.HasOne<SykiTeacher>().WithMany().HasForeignKey(td => td.TeacherId)
            );
    }
}
