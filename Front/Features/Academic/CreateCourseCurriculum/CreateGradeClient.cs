namespace Syki.Front.Features.Academic.CreateCourseCurriculum;

public class CreateGradeClient(HttpClient http)
{
    public async Task<HttpResponseMessage> Create(string name, Guid cursoId, List<GradeDisciplinaIn> disciplinas)
    {
        var data = new GradeIn
        {
            Name = name,
            CursoId = cursoId,
            Disciplinas = disciplinas
        };
        return await http.PostAsJsonAsync("/grades", data);
    }
}
