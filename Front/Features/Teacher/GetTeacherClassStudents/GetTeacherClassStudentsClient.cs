namespace Syki.Front.Features.Teacher.GetTeacherClassStudents;

public class GetTeacherClassStudentsClient(HttpClient http) : ITeacherClient
{
    public async Task<OneOf<List<TeacherClassStudentOut>, ErrorOut>> Get(Guid classId)
    {
        var response = await http.GetAsync($"teacher/classes/{classId}/students");

        return await response.Resolve<List<TeacherClassStudentOut>>();
    }
}
