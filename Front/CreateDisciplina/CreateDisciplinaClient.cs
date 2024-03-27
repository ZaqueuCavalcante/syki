namespace Front.CreateDisciplina;

public class CreateDisciplinaClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(string name, List<Guid> cursos)
    {
        var data = new DisciplinaIn { Nome = name, Cursos = cursos, Code = name[..5] };
        return await http.PostAsJsonAsync("/disciplinas", data);
    }
}
