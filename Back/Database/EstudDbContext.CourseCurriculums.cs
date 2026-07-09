using Estud.Back.Domain.CourseCurriculums;
using Estud.Back.Database.CourseCurriculums;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    public DbSet<CourseCurriculum> CourseCurriculums { get; set; }
    public DbSet<CourseCurriculumDiscipline> CourseCurriculumDisciplines { get; set; }

    private static void ConfigureCourseCurriculums(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CourseCurriculumDbConfig());
        modelBuilder.ApplyConfiguration(new CourseCurriculumDisciplineDbConfig());
    }
}
