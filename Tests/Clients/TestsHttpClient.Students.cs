using System.Net.Http.Json;
using Syki.Back.Features.Students.GetStudents;
using Syki.Back.Features.Students.CreateStudent;

namespace Syki.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<CreateStudentOut, ErrorOut>> CreateStudent(
        string name,
        string email
    ) {
        var data = new CreateStudentIn { Name = name, Email = email };
        var response = await http.PostAsJsonAsync("/students", data);
        return await response.Resolve<CreateStudentOut>();
    }

    public async Task<GetStudentsOut> GetStudents()
    {
        return await http.GetFromJsonAsync<GetStudentsOut>("/students", JsonOptions) ?? new();
    }
}
