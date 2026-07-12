using Estud.Back.Domain.Calendar;
using Estud.Back.Database.Calendar;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    public DbSet<CalendarDay> CalendarDays { get; set; }

    private static void ConfigureCalendar(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CalendarDayDbConfig());
    }
}
