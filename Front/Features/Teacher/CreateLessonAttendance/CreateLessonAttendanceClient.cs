namespace Syki.Front.Features.Teacher.CreateLessonAttendance;

public class CreateLessonAttendanceClient(HttpClient http) : ITeacherClient
{
    public async Task<OneOf<SuccessOut, ErrorOut>> Create(Guid id, List<Guid> presentStudents)
    {
        var data = new CreateLessonAttendanceIn(presentStudents);
        var response = await http.PutAsJsonAsync($"/teacher/lessons/{id}/attendance", data);
        return await response.Resolve<SuccessOut>();
    }
}
