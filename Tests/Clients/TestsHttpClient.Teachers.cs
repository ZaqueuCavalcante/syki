using System.Net.Http.Json;
using Syki.Back.Features.Teachers.GetTeacher;
using Syki.Back.Features.Teachers.GetTeachers;
using Syki.Back.Features.Teachers.CreateTeacher;
using Syki.Back.Features.Teachers.AssignCampiToTeacher;
using Syki.Back.Features.Teachers.AssignDisciplinesToTeacher;

namespace Syki.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<CreateTeacherOut, ErrorOut>> CreateTeacher(
        string name,
        string email
    ) {
        var data = new CreateTeacherIn { Name = name, Email = email };
        var response = await http.PostAsJsonAsync("/teachers", data);
        return await response.Resolve<CreateTeacherOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> AssignCampiToTeacher(
        int teacherId,
        List<int> campi
    ) {
        var data = new AssignCampiToTeacherIn { Campi = campi };
        var response = await http.PutAsJsonAsync($"/teachers/{teacherId}/assign-campi", data);
        return await response.Resolve<SuccessOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> AssignDisciplinesToTeacher(
        int teacherId,
        List<int> disciplines
    ) {
        var data = new AssignDisciplinesToTeacherIn { Disciplines = disciplines };
        var response = await http.PutAsJsonAsync($"/teachers/{teacherId}/assign-disciplines", data);
        return await response.Resolve<SuccessOut>();
    }

    public async Task<OneOf<GetTeachersOut, ErrorOut>> GetTeachers()
    {
        var response = await http.GetAsync("/teachers");
        return await response.Resolve<GetTeachersOut>();
    }

    public async Task<OneOf<GetTeacherOut, ErrorOut>> GetTeacher(int id)
    {
        var response = await http.GetAsync($"/teachers/{id}");
        return await response.Resolve<GetTeacherOut>();
    }
}
