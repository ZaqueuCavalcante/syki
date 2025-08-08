namespace Syki.Front.Features.Academic.GetAcademicTeacher;

public class GetAcademicTeacherClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<GetAcademicTeacherOut, ErrorOut>> Get(Guid id)
    {
        var response = await http.GetAsync($"/academic/teachers/{id}");

        return await response.Resolve<GetAcademicTeacherOut>();
    }
}
