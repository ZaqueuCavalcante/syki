namespace Front.CreateCurso;

public class CreateCursoClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(string name, TipoDeCurso type)
    {
        var data = new CursoIn { Nome = name, Tipo = type };
        return await http.PostAsJsonAsync("/cursos", data);
    }
}
