using System.Net.Http.Json;
using Syki.Back.Features.Students.GetStudent;
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

    public async Task<OneOf<GetStudentsOut, ErrorOut>> GetStudents()
    {
        var response = await http.GetAsync("/students");
        return await response.Resolve<GetStudentsOut>();
    }

    public async Task<OneOf<GetStudentOut, ErrorOut>> GetStudent(int id)
    {
        var response = await http.GetAsync($"/students/{id}");
        return await response.Resolve<GetStudentOut>();
    }
}
