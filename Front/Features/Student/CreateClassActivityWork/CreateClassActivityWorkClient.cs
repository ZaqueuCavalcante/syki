namespace Syki.Front.Features.Student.CreateClassActivityWork;

public class CreateClassActivityWorkClient(HttpClient http) : IStudentClient
{
    public async Task<OneOf<ClassActivityWorkOut, ErrorOut>> Create(Guid id, string link)
    {
        var data = new CreateClassActivityWorkIn { Link = link };

        var response = await http.PostAsJsonAsync($"/student/activities/{id}/works", data);

        return await response.Resolve<ClassActivityWorkOut>();
    }
}
