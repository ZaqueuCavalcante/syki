using Syki.Back.Domain.Periods;
using Syki.Back.Database.Periods;

namespace Syki.Back.Database;

public partial class SykiDbContext
{
    public DbSet<AcademicPeriod> AcademicPeriods { get; set; }

    private static void ConfigurePeriods(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AcademicPeriodDbConfig());
        modelBuilder.ApplyConfiguration(new EnrollmentPeriodDbConfig());
    }
}
