namespace Syki.Front.Features.Teacher.GetTeacherLessonAttendances;

public class GetTeacherLessonAttendancesClient(HttpClient http) : ITeacherClient
{
    public async Task<List<GetTeacherLessonAttendanceOut>> Get(Guid id)
    {
        return await http.GetFromJsonAsync<List<GetTeacherLessonAttendanceOut>>($"/teacher/lessons/{id}/attendances", HttpConfigs.JsonOptions) ?? [];
    }
}
