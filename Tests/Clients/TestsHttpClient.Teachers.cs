using System.Net.Http.Json;
using Syki.Back.Features.Teachers.CreateTeacher;

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
}
