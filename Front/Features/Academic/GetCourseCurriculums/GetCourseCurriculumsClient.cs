namespace Syki.Front.Features.Academic.GetCourseCurriculums;

public class GetCourseCurriculumsClient(HttpClient http)
{
    public async Task<List<CourseCurriculumOut>> Get()
    {
        return await http.GetFromJsonAsync<List<CourseCurriculumOut>>("/academic/course-curriculums") ?? [];
    }
}
