namespace Syki.Front.Features.Academic.GetAcademicClasses;

public class GetAcademicClassesClient(HttpClient http) : IAcademicClient
{
    public async Task<List<ClassOut>> Get(GetAcademicClassesIn? query = null)
    {
        var queryString = query?.Status != null ? $"?status={query.Status}" : "";
        return await http.GetFromJsonAsync<List<ClassOut>>($"/academic/classes{queryString}", HttpConfigs.JsonOptions) ?? [];
    }
}
