namespace Syki.Front.Features.Academic.CreateCourseCurriculum;

public class CreateGradeClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(string name, Guid cursoId, List<CreateCourseCurriculumDisciplineIn> disciplines)
    {
        var data = new CreateCourseCurriculumIn
        {
            Name = name,
            CourseId = cursoId,
            Disciplines = disciplines
        };
        return await http.PostAsJsonAsync("/grades", data);
    }
}
