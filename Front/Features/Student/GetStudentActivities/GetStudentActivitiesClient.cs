namespace Syki.Front.Features.Student.GetStudentActivities;

public class GetStudentActivitiesClient(HttpClient http) : IStudentClient
{
    public async Task<List<StudentActivityOut>> Get()
    {
        return await http.GetFromJsonAsync<List<StudentActivityOut>>("/student/activities", HttpConfigs.JsonOptions) ?? [];
    }
}
