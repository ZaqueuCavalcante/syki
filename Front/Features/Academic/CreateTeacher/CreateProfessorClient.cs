namespace Syki.Front.Features.Academic.CreateTeacher;

public class CreateProfessorClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(string name, string email)
    {
        var data = new ProfessorIn { Name = name, Email = email };
        return await http.PostAsJsonAsync("/teachers", data);
    }
}
