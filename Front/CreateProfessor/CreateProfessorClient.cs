namespace Front.CreateProfessor;

public class CreateProfessorClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(string name, string email)
    {
        var data = new ProfessorIn { Nome = name, Email = email };
        return await http.PostAsJsonAsync("/professores", data);
    }
}
