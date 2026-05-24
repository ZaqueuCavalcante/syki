using System.Net.Http.Json;
using Syki.Back.Features.Courses.GetCourses;
using Syki.Back.Features.Courses.CreateCourse;
using Syki.Back.Features.Courses.UpdateCourse;

namespace Syki.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<CreateCourseOut, ErrorOut>> CreateCourse(
        string name = "Análise e Desenvolvimento de Sistemas",
        CourseType? type = CourseType.Tecnologo
    ) {
        var data = new CreateCourseIn { Name = name, Type = type };
        var response = await http.PostAsJsonAsync("/courses", data);
        return await response.Resolve<CreateCourseOut>();
    }

    public async Task<OneOf<UpdateCourseOut, ErrorOut>> UpdateCourse(
        int id,
        string name = "Direito",
        CourseType? type = CourseType.Bacharelado
    ) {
        var data = new UpdateCourseIn { Id = id, Name = name, Type = type };
        var response = await http.PutAsJsonAsync("/courses", data);
        return await response.Resolve<UpdateCourseOut>();
    }

    public async Task<GetCoursesOut> GetCourses()
    {
        var response = await http.GetAsync("/courses");
        return await response.DeserializeTo<GetCoursesOut>();
    }
}
