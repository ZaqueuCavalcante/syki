namespace Syki.Front.Features.Teacher.SetSchedulingPreferences;

public class SetSchedulingPreferencesClient(HttpClient http) : ITeacherClient
{
    public async Task<OneOf<SuccessOut, ErrorOut>> Set(List<ScheduleIn> schedules)
    {
        var data = new SetSchedulingPreferencesIn { Schedules = schedules };

        var response = await http.PutAsJsonAsync($"teacher/scheduling-preferences", data);

        return await response.Resolve<SuccessOut>();
    }
}
