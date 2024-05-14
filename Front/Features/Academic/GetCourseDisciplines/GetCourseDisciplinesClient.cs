namespace Syki.Front.Features.Academic.GetCourseDisciplines;

public class GetCourseDisciplinesClient(HttpClient http)
{
    public async Task<List<CourseDisciplineOut>> Get(Guid courseId)
    {
        return await http.GetFromJsonAsync<List<CourseDisciplineOut>>($"/academic/courses/{courseId}/disciplines") ?? [];
    }
}
