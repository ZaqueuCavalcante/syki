namespace Syki.Front.Features.Teacher.CreateClassActivity;

public class CreateClassActivityClient(HttpClient http) : ITeacherClient
{
    public async Task<OneOf<SuccessOut, ErrorOut>> Create(Guid classId, Guid lessonId, string title, string description, DateTime? dueDate)
    {
        var data = new CreateClassActivityIn { LessonId = lessonId, Title = title, Description = description, DueDate = dueDate };

        var response = await http.PostAsJsonAsync($"/teacher/classes/{classId}/activities", data);

        return await response.Resolve<SuccessOut>();
    }
}
