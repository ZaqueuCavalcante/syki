namespace Front.CreateCurso;

public class CreateCursoClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(string name, TipoDeCurso type)
    {
        var data = new CursoIn { Name = name, Tipo = type };
        return await http.PostAsJsonAsync("/cursos", data);
    }
}
