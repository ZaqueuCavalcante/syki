using System.Net.Http.Json;
using Estud.Back.Features.Courses.GetCourse;
using Estud.Back.Features.Courses.GetCourses;
using Estud.Back.Features.Courses.CreateCourse;
using Estud.Back.Features.Courses.UpdateCourse;
using Estud.Back.Features.Courses.AddCourseDisciplines;
using Estud.Back.Features.Courses.GetCourseDisciplines;
using Estud.Back.Features.Courses.RemoveCourseDiscipline;
using Estud.Back.Features.Courses.GetCoursePotentialDisciplines;

namespace Estud.Tests.Integration.Clients;

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

    public async Task<OneOf<GetCoursesOut, ErrorOut>> GetCourses(int? page = null, int? pageSize = null)
    {
        var data = new GetCoursesIn
        {
            Page = page ?? 1,
            PageSize = pageSize ?? 10,
        };

        var response = await http.GetAsync("/courses".AddQueryString(data));
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
        var url = $"/courses/{id}/potential-disciplines" + (name.IsEmpty() ? "" : $"?name={name}");
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
