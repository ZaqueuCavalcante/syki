using Estud.Back.Domain.CourseOfferings;
using Estud.Back.Database.CourseOfferings;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    public DbSet<CourseOffering> CourseOfferings { get; set; }

    private static void ConfigureCourseOfferings(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CourseOfferingDbConfig());
    }
}
