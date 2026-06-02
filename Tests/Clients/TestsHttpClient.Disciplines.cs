using System.Net.Http.Json;
using Syki.Back.Features.Disciplines.GetDisciplines;
using Syki.Back.Features.Disciplines.CreateDiscipline;
using Syki.Back.Features.Disciplines.UpdateDiscipline;
using Syki.Back.Features.Disciplines.GetDiscipline;
using Syki.Back.Features.Disciplines.AddDisciplineCourses;

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

    public async Task<GetDisciplinesOut> GetDisciplines()
    {
        var response = await http.GetAsync("/disciplines");
        return await response.DeserializeTo<GetDisciplinesOut>();
    }
}
