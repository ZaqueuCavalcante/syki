namespace Syki.Front.Features.Teacher.GetTeacherClassActivity;

public class GetTeacherClassActivityClient(HttpClient http) : ITeacherClient
{
    public async Task<OneOf<TeacherClassActivityOut, ErrorOut>> Get(Guid classId, Guid activityId)
    {
        var response = await http.GetAsync($"teacher/classes/{classId}/activities/{activityId}");

        return await response.Resolve<TeacherClassActivityOut>();
    }
}
