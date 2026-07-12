using Estud.Back.Domain.Calendar;

namespace Estud.Back.Features.Calendar.GetInstitutionCalendar;

public class GetInstitutionCalendarService(EstudDbContext ctx) : IEstudService
{
    public async Task<GetInstitutionCalendarOut> Get(GetInstitutionCalendarIn data)
    {
        var year = data.Year ?? DateTime.UtcNow.Year;

        var start = new DateOnly(year, 1, 1);
        var end = new DateOnly(year, 12, 31);

        var customDays = await ctx.CalendarDays.AsNoTracking()
            .Where(d => d.InstitutionId == ctx.RequestUser.InstitutionId)
            .Where(d => d.Date >= start && d.Date <= end)
            .ToDictionaryAsync(d => d.Date);

        var holidays = NationalHolidays.OfYear(year);

        var items = new List<GetInstitutionCalendarItemOut>();
        for (var date = start; date <= end; date = date.AddDays(1))
        {
            if (customDays.TryGetValue(date, out var customDay))
            {
                items.Add(customDay.ToGetInstitutionCalendarItemOut());
                continue;
            }

            var isHoliday = holidays.TryGetValue(date, out var holiday);

            items.Add(new GetInstitutionCalendarItemOut
            {
                Date = date.ToDateTime(TimeOnly.MinValue),
                DayType = isHoliday ? DayType.Holiday : DayType.Default,
                Description = holiday,
            });
        }

        return new GetInstitutionCalendarOut
        {
            Year = year,
            Total = items.Count,
            Items = items,
        };
    }
}
