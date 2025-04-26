namespace Syki.Front.Features.Teacher.GetTeacherClass;

public class GetTeacherClassClient(HttpClient http) : ITeacherClient
{
    public async Task<OneOf<TeacherClassOut, ErrorOut>> Get(Guid id)
    {
        var response = await http.GetAsync($"/teacher/classes/{id}");

        return await response.Resolve<TeacherClassOut>();
    }
}
