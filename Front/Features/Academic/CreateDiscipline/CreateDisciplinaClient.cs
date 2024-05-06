namespace Syki.Front.Features.Academic.CreateDiscipline;

public class CreateDisciplinaClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(string name, List<Guid> cursos)
    {
        var data = new DisciplinaIn { Name = name, Cursos = cursos };
        return await http.PostAsJsonAsync("/disciplinas", data);
    }
}
