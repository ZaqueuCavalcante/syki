namespace Syki.Front.Features.Student.GetStudentClassActivities;

public class GetStudentClassActivitiesClient(HttpClient http) : IStudentClient
{
    public async Task<List<StudentClassActivityOut>> Get(Guid id)
    {
        return await http.GetFromJsonAsync<List<StudentClassActivityOut>>($"/student/classes/{id}/activities", HttpConfigs.JsonOptions) ?? [];
    }
}
