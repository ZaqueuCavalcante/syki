using Syki.Back.Domain.CourseOfferings;
using Syki.Back.Database.CourseOfferings;

namespace Syki.Back.Database;

public partial class SykiDbContext
{
    public DbSet<CourseOffering> CourseOfferings { get; set; }

    private static void ConfigureCourseOfferings(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CourseOfferingDbConfig());
    }
}
