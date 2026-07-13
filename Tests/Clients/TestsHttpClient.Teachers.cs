using System.Net.Http.Json;
using Estud.Back.Features.Teachers.GetTeacher;
using Estud.Back.Features.Teachers.GetTeachers;
using Estud.Back.Features.Teachers.CreateTeacher;
using Estud.Back.Features.Teachers.UpdateTeacher;
using Estud.Back.Features.Teachers.GetTeacherClass;
using Estud.Back.Features.Teachers.AssignCampiToTeacher;
using Estud.Back.Features.Teachers.GetTeacherCurrentClasses;
using Estud.Back.Features.Teachers.GetTeacherPotentialCampi;
using Estud.Back.Features.Teachers.AssignDisciplinesToTeacher;
using Estud.Back.Features.Teachers.GetTeacherPotentialDisciplines;

namespace Estud.Tests.Integration.Clients;

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

    public async Task<OneOf<GetTeacherPotentialCampiOut, ErrorOut>> GetTeacherPotentialCampi(int id, string? name = null)
    {
        var url = $"/teachers/{id}/potential-campi";
        if (name != null) url += $"?name={name}";
        var response = await http.GetAsync(url);
        return await response.Resolve<GetTeacherPotentialCampiOut>();
    }

    public async Task<OneOf<GetTeacherPotentialDisciplinesOut, ErrorOut>> GetTeacherPotentialDisciplines(int id, string? name = null)
    {
        var url = $"/teachers/{id}/potential-disciplines";
        if (name != null) url += $"?name={name}";
        var response = await http.GetAsync(url);
        return await response.Resolve<GetTeacherPotentialDisciplinesOut>();
    }

    public async Task<OneOf<GetTeacherCurrentClassesOut, ErrorOut>> GetTeacherCurrentClasses()
    {
        var response = await http.GetAsync("/teachers/current-classes");
        return await response.Resolve<GetTeacherCurrentClassesOut>();
    }

    public async Task<OneOf<GetTeacherClassOut, ErrorOut>> GetTeacherClass(int id)
    {
        var response = await http.GetAsync($"/teachers/classes/{id}");
        return await response.Resolve<GetTeacherClassOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> UpdateTeacher(
        int id,
        string name = "Richard Feynman",
        string email = "feynman@estud.com"
    ) {
        var data = new UpdateTeacherIn { Name = name, Email = email };
        var response = await http.PutAsJsonAsync($"/teachers/{id}", data);
        return await response.Resolve<SuccessOut>();
    }
}
