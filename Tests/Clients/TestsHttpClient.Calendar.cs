using System.Net.Http.Json;
using Estud.Back.Features.Calendar.CreateCalendarDay;
using Estud.Back.Features.Calendar.UpdateCalendarDay;
using Estud.Back.Features.Calendar.GetInstitutionCalendar;

namespace Estud.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<GetInstitutionCalendarOut, ErrorOut>> GetInstitutionCalendar(int? year = null)
    {
        var data = new GetInstitutionCalendarIn { Year = year };
        var response = await http.GetAsync("calendar/institution".AddQueryString(data));
        return await response.Resolve<GetInstitutionCalendarOut>();
    }

    public async Task<OneOf<CreateCalendarDayOut, ErrorOut>> CreateCalendarDay(
        DateTime? date = null,
        DayType? dayType = DayType.Vacation,
        string? description = "Férias de verão"
    ) {
        var data = new CreateCalendarDayIn
        {
            Date = date ?? new DateTime(2026, 1, 5),
            DayType = dayType,
            Description = description,
        };
        var response = await http.PostAsJsonAsync("calendar/days", data);
        return await response.Resolve<CreateCalendarDayOut>();
    }

    public async Task<OneOf<UpdateCalendarDayOut, ErrorOut>> UpdateCalendarDay(
        int id,
        DayType? dayType = DayType.Recess,
        string? description = "Recesso de fim de ano"
    ) {
        var data = new UpdateCalendarDayIn
        {
            Id = id,
            DayType = dayType,
            Description = description,
        };
        var response = await http.PutAsJsonAsync("calendar/days", data);
        return await response.Resolve<UpdateCalendarDayOut>();
    }
}
