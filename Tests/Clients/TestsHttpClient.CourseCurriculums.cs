using System.Net.Http.Json;
using Syki.Back.Features.CourseCurriculums.GetCourseCurriculum;
using Syki.Back.Features.CourseCurriculums.EditCourseCurriculum;
using Syki.Back.Features.CourseCurriculums.GetCourseCurriculums;
using Syki.Back.Features.CourseCurriculums.CreateCourseCurriculum;

namespace Syki.Tests.Integration.Clients;

public partial class TestsHttpClient
{
    public async Task<OneOf<CreateCourseCurriculumOut, ErrorOut>> CreateCourseCurriculum(
        int courseId,
        string name = "Grade 2024",
        List<CreateCourseCurriculumDisciplineIn>? disciplines = null
    ) {
        var data = new CreateCourseCurriculumIn { Name = name, CourseId = courseId, Disciplines = disciplines ?? [] };
        var response = await http.PostAsJsonAsync("/course-curriculums", data);
        return await response.Resolve<CreateCourseCurriculumOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> EditCourseCurriculum(
        int id,
        string name = "Grade 2024",
        List<EditCourseCurriculumDisciplineIn>? disciplines = null
    ) {
        var data = new EditCourseCurriculumIn { Id = id, Name = name, Disciplines = disciplines ?? [] };
        var response = await http.PutAsJsonAsync("/course-curriculums", data);
        return await response.Resolve<SuccessOut>();
    }

    public async Task<OneOf<GetCourseCurriculumOut, ErrorOut>> GetCourseCurriculum(int id)
    {
        var response = await http.GetAsync($"/course-curriculums/{id}");
        return await response.Resolve<GetCourseCurriculumOut>();
    }

    public async Task<OneOf<GetCourseCurriculumsOut, ErrorOut>> GetCourseCurriculums()
    {
        var response = await http.GetAsync("/course-curriculums");
        return await response.Resolve<GetCourseCurriculumsOut>();
    }
}
