namespace Syki.Front.Features.Academic.GetClasses;

public class GetClassesClient(HttpClient http)
{
    public async Task<List<ClassOut>> Get()
    {
        return await http.GetFromJsonAsync<List<ClassOut>>("/academic/classes") ?? [];
    }
}
