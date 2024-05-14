namespace Syki.Front.Features.Academic.GetCoursesWithDisciplines;

public class GetCoursesWithDisciplinesClient(HttpClient http)
{
    public async Task<List<CourseOut>> Get()
    {
        return await http.GetFromJsonAsync<List<CourseOut>>("/academic/courses/with-disciplines") ?? [];
    }
}
