namespace Syki.Front.Features.Academic.GetCoursesWithCurriculums;

public class GetCoursesWithCurriculumsClient(HttpClient http)
{
    public async Task<List<CourseOut>> Get()
    {
        return await http.GetFromJsonAsync<List<CourseOut>>("/academic/courses/with-curriculums") ?? [];
    }
}
