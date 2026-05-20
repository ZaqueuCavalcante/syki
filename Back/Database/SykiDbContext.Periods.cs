using Syki.Back.Database.Periods;

namespace Syki.Back.Database;

public partial class SykiDbContext
{
    private static void ConfigurePeriods(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AcademicPeriodDbConfig());
        modelBuilder.ApplyConfiguration(new EnrollmentPeriodDbConfig());
    }
}
