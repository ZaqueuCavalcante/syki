using Estud.Back.Domain.Periods;
using Estud.Back.Database.Periods;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    public DbSet<AcademicPeriod> AcademicPeriods { get; set; }

    private static void ConfigurePeriods(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AcademicPeriodDbConfig());
        modelBuilder.ApplyConfiguration(new EnrollmentPeriodDbConfig());
    }
}
