namespace Syki.Front.Features.Teacher.GetTeacherClassActivities;

public class GetTeacherClassActivitiesClient(HttpClient http) : ITeacherClient
{
    public async Task<OneOf<List<TeacherClassActivityOut>, ErrorOut>> Get(Guid classId)
    {
        var response = await http.GetAsync($"teacher/classes/{classId}/activities");

        return await response.Resolve<List<TeacherClassActivityOut>>();
    }
}
