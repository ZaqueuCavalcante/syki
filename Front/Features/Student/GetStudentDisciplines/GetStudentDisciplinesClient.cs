namespace Syki.Front.Features.Student.GetStudentDisciplines;

public class GetStudentDisciplinesClient(HttpClient http)
{
    public async Task<List<DisciplinaOut>> Get()
    {
        return await http.GetFromJsonAsync<List<DisciplinaOut>>("students/disciplines") ?? [];
    }
}
