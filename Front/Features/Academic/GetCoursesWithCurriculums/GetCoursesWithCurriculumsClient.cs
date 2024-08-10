namespace Syki.Front.Features.Academic.GetCoursesWithCurriculums;

public class GetCoursesWithCurriculumsClient(HttpClient http) : IAcademicClient
{
    public async Task<List<CourseOut>> Get()
    {
        return await http.GetFromJsonAsync<List<CourseOut>>("/academic/courses/with-curriculums") ?? [];
    }
}
