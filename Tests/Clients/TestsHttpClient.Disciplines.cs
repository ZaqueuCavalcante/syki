using System.Net.Http.Json;
using Syki.Back.Features.Disciplines.GetDiscipline;
using Syki.Back.Features.Disciplines.GetDisciplines;
using Syki.Back.Features.Disciplines.CreateDiscipline;
using Syki.Back.Features.Disciplines.UpdateDiscipline;
using Syki.Back.Features.Disciplines.AddDisciplineCourses;
using Syki.Back.Features.Disciplines.RemoveDisciplineCourse;
using Syki.Back.Features.Disciplines.GetDisciplinePotentialCourses;

namespace Syki.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<CreateDisciplineOut, ErrorOut>> CreateDiscipline(
        string name = "Geometria"
    ) {
        var data = new CreateDisciplineIn { Name = name };
        var response = await http.PostAsJsonAsync("/disciplines", data);
        return await response.Resolve<CreateDisciplineOut>();
    }

    public async Task<OneOf<UpdateDisciplineOut, ErrorOut>> UpdateDiscipline(
        int id,
        string name = "Física II"
    ) {
        var data = new UpdateDisciplineIn { Id = id, Name = name };
        var response = await http.PutAsJsonAsync("/disciplines", data);
        return await response.Resolve<UpdateDisciplineOut>();
    }

    public async Task<OneOf<GetDisciplineOut, ErrorOut>> GetDiscipline(int id)
    {
        var response = await http.GetAsync($"/disciplines/{id}");
        return await response.Resolve<GetDisciplineOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> AddDisciplineCourses(
        int disciplineId,
        List<int> courses
    ) {
        var data = new AddDisciplineCoursesIn { DisciplineId = disciplineId, Courses = courses };
        var response = await http.PostAsJsonAsync("/disciplines/courses", data);
        return await response.Resolve<SuccessOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> RemoveDisciplineCourse(
        int disciplineId,
        int courseId
    ) {
        var data = new RemoveDisciplineCourseIn { DisciplineId = disciplineId, CourseId = courseId };
        var request = new HttpRequestMessage(HttpMethod.Delete, "/disciplines/courses")
        {
            Content = JsonContent.Create(data)
        };
        var response = await http.SendAsync(request);
        return await response.Resolve<SuccessOut>();
    }

    public async Task<OneOf<GetDisciplinesOut, ErrorOut>> GetDisciplines()
    {
        var response = await http.GetAsync("/disciplines");
        return await response.Resolve<GetDisciplinesOut>();
    }

    public async Task<OneOf<GetDisciplinePotentialCoursesOut, ErrorOut>> GetDisciplinePotentialCourses(
        int id,
        string? name = null
    ) {
        var url = $"/disciplines/{id}/potential-courses?name={name}";
        var response = await http.GetAsync(url);
        return await response.Resolve<GetDisciplinePotentialCoursesOut>();
    }
}
