namespace Syki.Front.Features.Student.CreateClassActivityWork;

public class CreateClassActivityWorkClient(HttpClient http) : IStudentClient
{
    public async Task<OneOf<ClassActivityWorkOut, ErrorOut>> Create(Guid activityId, string link)
    {
        var data = new CreateClassActivityWorkIn { Link = link };

        var response = await http.PostAsJsonAsync($"/student/activities/{activityId}/works", data);

        return await response.Resolve<ClassActivityWorkOut>();
    }
}
