using System.Net.Http.Json;
using Estud.Back.Features.Parents.GetParents;
using Estud.Back.Features.Parents.CreateParent;
using Estud.Back.Features.Parents.GetParentStudents;
using Estud.Back.Features.Parents.GetParentStudentAgenda;

namespace Estud.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<GetParentsOut, ErrorOut>> GetParents(
        string? filter = null,
        int? page = null,
        int? pageSize = null
    ) {
        var data = new GetParentsIn { Filter = filter, Page = page ?? 1, PageSize = pageSize ?? 10 };
        var response = await http.GetAsync("/parents".AddQueryString(data));
        return await response.Resolve<GetParentsOut>();
    }

    public async Task<OneOf<CreateParentOut, ErrorOut>> CreateParent(
        string name,
        string email,
        List<CreateParentStudentIn>? students = null,
        string? phoneNumber = null
    ) {
        var data = new CreateParentIn { Name = name, Email = email, PhoneNumber = phoneNumber, Students = students ?? [] };
        var response = await http.PostAsJsonAsync("/parents", data);
        return await response.Resolve<CreateParentOut>();
    }

    public async Task<OneOf<GetParentStudentsOut, ErrorOut>> GetParentStudents()
    {
        var response = await http.GetAsync("/parents/students");
        return await response.Resolve<GetParentStudentsOut>();
    }

    public async Task<OneOf<GetParentStudentAgendaOut, ErrorOut>> GetParentStudentAgenda(int studentId)
    {
        var response = await http.GetAsync($"/parents/students/{studentId}/agenda");
        return await response.Resolve<GetParentStudentAgendaOut>();
    }
}
