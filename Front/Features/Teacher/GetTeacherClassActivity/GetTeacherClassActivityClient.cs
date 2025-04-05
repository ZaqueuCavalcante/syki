namespace Syki.Front.Features.Teacher.GetTeacherClassActivity;

public class GetTeacherClassActivityClient(HttpClient http) : ITeacherClient
{
    public async Task<TeacherClassActivityOut> Get(Guid classId, Guid activityId)
    {
        return await http.GetFromJsonAsync<TeacherClassActivityOut>($"teacher/classes/{classId}/activities/{activityId}", HttpConfigs.JsonOptions) ?? new();
    }
}
