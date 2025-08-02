namespace Syki.Front.Features.Academic.AddDisciplinePreRequisites;

public class AddDisciplinePreRequisitesClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<SuccessOut, ErrorOut>> Add(Guid courseCurriculumId, Guid disciplineId, List<Guid> preRequisites)
    {
        var data = new AddDisciplinePreRequisitesIn { DisciplineId = disciplineId, PreRequisites = preRequisites };

        var response = await http.PostAsJsonAsync($"/academic/course-curriculums/{courseCurriculumId}/pre-requisites", data);

        return await response.Resolve<SuccessOut>();
    }
}
