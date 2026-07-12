using Estud.Back.Domain.Calendar;

namespace Estud.Back.Features.Calendar.GetInstitutionCalendar;

public static class GetInstitutionCalendarMapper
{
    extension(CalendarDay day)
    {
        public GetInstitutionCalendarItemOut ToGetInstitutionCalendarItemOut()
        {
            return new()
            {
                Id = day.Id,
                Date = day.Date.ToDateTime(TimeOnly.MinValue),
                DayType = day.DayType,
                Description = day.Description,
            };
        }
    }
}
