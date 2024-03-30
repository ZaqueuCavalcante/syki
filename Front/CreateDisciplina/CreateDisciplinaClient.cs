namespace Front.CreateDisciplina;

public class CreateDisciplinaClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(string name, string code, List<Guid> cursos)
    {
        var data = new DisciplinaIn { Nome = name, Cursos = cursos, Code = code };
        return await http.PostAsJsonAsync("/disciplinas", data);
    }
}
