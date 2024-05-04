namespace Front.GetCursos;

public class GetCursosClient(HttpClient http)
{
    public async Task<List<CursoOut>> Get()
    {
        return await http.GetFromJsonAsync<List<CursoOut>>("/cursos") ?? [];
    }
}
