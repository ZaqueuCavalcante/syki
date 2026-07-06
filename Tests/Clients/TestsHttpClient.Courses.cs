using System.Net.Http.Json;
using Syki.Back.Features.Courses.GetCourse;
using Syki.Back.Features.Courses.GetCourses;
using Syki.Back.Features.Courses.CreateCourse;
using Syki.Back.Features.Courses.UpdateCourse;
using Syki.Back.Features.Courses.AddCourseDisciplines;
using Syki.Back.Features.Courses.GetCourseDisciplines;
using Syki.Back.Features.Courses.RemoveCourseDiscipline;
using Syki.Back.Features.Courses.GetCoursePotentialDisciplines;

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

    public async Task<OneOf<GetCourseOut, ErrorOut>> GetCourse(int id)
    {
        var response = await http.GetAsync($"/courses/{id}");
        return await response.Resolve<GetCourseOut>();
    }

    public async Task<OneOf<GetCoursesOut, ErrorOut>> GetCourses()
    {
        var response = await http.GetAsync("/courses");
        return await response.Resolve<GetCoursesOut>();
    }

    public async Task<OneOf<GetCourseDisciplinesOut, ErrorOut>> GetCourseDisciplines(int id)
    {
        var response = await http.GetAsync($"/courses/{id}/disciplines");
        return await response.Resolve<GetCourseDisciplinesOut>();
    }

    public async Task<OneOf<GetCoursePotentialDisciplinesOut, ErrorOut>> GetCoursePotentialDisciplines(
        int id,
        string? name = null
    ) {
        var url = name == null ? $"/courses/{id}/potential-disciplines" : $"/courses/{id}/potential-disciplines?name={name}";
        var response = await http.GetAsync(url);
        return await response.Resolve<GetCoursePotentialDisciplinesOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> AddCourseDisciplines(int courseId, List<int> disciplines)
    {
        var data = new AddCourseDisciplinesIn { CourseId = courseId, Disciplines = disciplines };
        var response = await http.PostAsJsonAsync("/courses/disciplines", data);
        return await response.Resolve<SuccessOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> RemoveCourseDiscipline(int courseId, int disciplineId)
    {
        var data = new RemoveCourseDisciplineIn { CourseId = courseId, DisciplineId = disciplineId };
        var request = new HttpRequestMessage(HttpMethod.Delete, "/courses/disciplines")
        {
            Content = JsonContent.Create(data),
        };
        var response = await http.SendAsync(request);
        return await response.Resolve<SuccessOut>();
    }
}
