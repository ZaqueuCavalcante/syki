namespace Syki.Front.Features.Academic.CreateDiscipline;

public class CreateDisciplineClient(HttpClient http) : IAcademicClient
{
    public async Task<HttpResponseMessage> Create(string name, List<Guid> courses)
    {
        var data = new CreateDisciplineIn { Name = name, Courses = courses };
        return await http.PostAsJsonAsync("/academic/disciplines", data);
    }
}
