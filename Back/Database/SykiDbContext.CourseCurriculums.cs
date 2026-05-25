using Syki.Back.Domain.CourseCurriculums;
using Syki.Back.Database.CourseCurriculums;

namespace Syki.Back.Database;

public partial class SykiDbContext
{
    public DbSet<CourseCurriculum> CourseCurriculums { get; set; }
    public DbSet<CourseCurriculumDiscipline> CourseCurriculumDisciplines { get; set; }

    private static void ConfigureCourseCurriculums(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CourseCurriculumDbConfig());
        modelBuilder.ApplyConfiguration(new CourseCurriculumDisciplineDbConfig());
    }
}
