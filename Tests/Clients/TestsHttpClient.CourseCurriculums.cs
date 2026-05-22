using System.Net.Http.Json;
using Syki.Back.Features.CourseCurriculums.CreateCourseCurriculum;
using Syki.Back.Features.CourseCurriculums.GetCourseCurriculums;

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

    public async Task<GetCourseCurriculumsOut> GetCourseCurriculums()
    {
        var response = await http.GetAsync("/course-curriculums");
        return await response.DeserializeTo<GetCourseCurriculumsOut>();
    }
}
