namespace Syki.Front.Features.Student.GetStudentDisciplines;

public class GetStudentDisciplinesClient(HttpClient http) : IStudentClient
{
    public async Task<List<DisciplineOut>> Get()
    {
        return await http.GetFromJsonAsync<List<DisciplineOut>>("/student/disciplines", HttpConfigs.JsonOptions) ?? [];
    }
}
