namespace Syki.Front.Features.Academic.AssignDisciplinesToTeacher;

public class AssignDisciplinesToTeacherClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<SuccessOut, ErrorOut>> Assign(Guid teacherId, List<Guid> disciplines)
    {
        var data = new AssignDisciplinesToTeacherIn { Disciplines = disciplines };

        var response = await http.PutAsJsonAsync($"/academic/teachers/{teacherId}/assign-disciplines", data);

        return await response.Resolve<SuccessOut>();
    }
}
