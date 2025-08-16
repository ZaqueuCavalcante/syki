namespace Syki.Front.Features.Academic.AssignClassToClassroom;

public class AssignClassToClassroomClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<SuccessOut, ErrorOut>> Assign(Guid classroomId, Guid classId, List<ScheduleIn> schedules)
    {
        var data = new AssignClassToClassroomIn { ClassId = classId, Schedules = schedules };

        var response = await http.PutAsJsonAsync($"/academic/classrooms/{classroomId}/assign-class", data);

        return await response.Resolve<SuccessOut>();
    }
}
