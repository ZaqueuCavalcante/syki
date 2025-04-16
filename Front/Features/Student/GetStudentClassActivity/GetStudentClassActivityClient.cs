namespace Syki.Front.Features.Student.GetStudentClassActivity;

public class GetStudentClassActivityClient(HttpClient http) : ITeacherClient
{
    public async Task<StudentClassActivityOut> Get(Guid classId, Guid activityId)
    {
        return await http.GetFromJsonAsync<StudentClassActivityOut>($"student/classes/{classId}/activities/{activityId}", HttpConfigs.JsonOptions) ?? new();
    }
}
