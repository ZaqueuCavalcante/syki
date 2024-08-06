namespace Syki.Front.Features.Academic.CreateTeacher;

public class CreateTeacherClient(HttpClient http)
{
    public async Task<OneOf<TeacherOut, ErrorOut>> Create(string name, string email)
    {
        var data = new CreateTeacherIn { Name = name, Email = email };

        var response = await http.PostAsJsonAsync("/academic/teachers", data);

        return await response.Resolve<TeacherOut>();
    }
}
