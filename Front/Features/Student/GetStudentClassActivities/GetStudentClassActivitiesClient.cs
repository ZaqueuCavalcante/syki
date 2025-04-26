namespace Syki.Front.Features.Student.GetStudentClassActivities;

public class GetStudentClassActivitiesClient(HttpClient http) : IStudentClient
{
    public async Task<OneOf<List<StudentClassActivityOut>, ErrorOut>> Get(Guid id)
    {
        var response = await http.GetAsync($"/student/classes/{id}/activities");

        return await response.Resolve<List<StudentClassActivityOut>>();
    }
}
