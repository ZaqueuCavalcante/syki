namespace Syki.Front.Features.Academic.CreateClassroom;

public class CreateClassroomClient(HttpClient http) : IAcademicClient
{
    public async Task<OneOf<CreateClassroomOut, ErrorOut>> Create(Guid campusId, string name, int capacity)
    {
        var data = new CreateClassroomIn { CampusId = campusId, Name = name, Capacity = capacity };

        var response = await http.PostAsJsonAsync("/academic/classrooms", data);

        return await response.Resolve<CreateClassroomOut>();
    }
}
