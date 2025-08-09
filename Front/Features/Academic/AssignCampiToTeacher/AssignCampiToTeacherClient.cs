namespace Syki.Front.Features.Academic.AssignCampiToTeacher;

public class AssignCampiToTeacherClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<SuccessOut, ErrorOut>> Assign(Guid teacherId, List<Guid> campi)
    {
        var data = new AssignCampiToTeacherIn { Campi = campi };

        var response = await http.PutAsJsonAsync($"/academic/teachers/{teacherId}/assign-campi", data);

        return await response.Resolve<SuccessOut>();
    }
}
