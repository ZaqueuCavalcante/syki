using Syki.Back.Domain.Disciplines;
using Syki.Back.Domain.CourseCurriculums;

namespace Syki.Back.Database.CourseCurriculums;

public class CourseCurriculumDbConfig : IEntityTypeConfiguration<CourseCurriculum>
{
    public void Configure(EntityTypeBuilder<CourseCurriculum> entity)
    {
        entity.ToTable("course_curriculums", DbSchemas.Syki);

        entity.HasKey(e => e.Id);

        entity.HasMany(e => e.Disciplines)
            .WithMany()
            .UsingEntity<CourseCurriculumDiscipline>(
                right => right.HasOne<Discipline>().WithMany().HasForeignKey(ccd => ccd.DisciplineId),
                left => left.HasOne<CourseCurriculum>().WithMany(cc => cc.Links).HasForeignKey(ccd => ccd.CourseCurriculumId)
            );
    }
}
