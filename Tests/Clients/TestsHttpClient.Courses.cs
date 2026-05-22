using System.Net.Http.Json;
using Syki.Back.Features.Courses.CreateCourse;

namespace Syki.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<CreateCourseOut, ErrorOut>> CreateCourse(
        string name = "Análise e Desenvolvimento de Sistemas",
        CourseType? type = CourseType.Tecnologo,
        List<string>? disciplines = null
    ) {
        var data = new CreateCourseIn { Name = name, Type = type, Disciplines = disciplines ?? [] };
        var response = await http.PostAsJsonAsync("/courses", data);
        return await response.Resolve<CreateCourseOut>();
    }
}
