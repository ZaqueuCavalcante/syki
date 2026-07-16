using System.Net.Http.Json;
using Estud.Back.Features.Parents.CreateParent;
using Estud.Back.Features.Parents.GetParentStudents;

namespace Estud.Tests.Integration.Clients;

public partial class TestsHttpClient
{
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
}
