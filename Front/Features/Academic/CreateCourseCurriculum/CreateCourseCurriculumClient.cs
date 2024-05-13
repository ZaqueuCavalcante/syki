namespace Syki.Front.Features.Academic.CreateCourseCurriculum;

public class CreateCourseCurriculumClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(string name, Guid courseId, List<CreateCourseCurriculumDisciplineIn> disciplines)
    {
        var data = new CreateCourseCurriculumIn
        {
            Name = name,
            CourseId = courseId,
            Disciplines = disciplines,
        };
        return await http.PostAsJsonAsync("/academic/course-curriculums", data);
    }
}
