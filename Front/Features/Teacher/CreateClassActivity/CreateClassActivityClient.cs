namespace Syki.Front.Features.Teacher.CreateClassActivity;

public class CreateClassActivityClient(HttpClient http) : ITeacherClient
{
    public async Task<OneOf<SuccessOut, ErrorOut>> Create(
        Guid classId,
        string title,
        string description,
        ClassActivityType type,
        int weight,
        DateOnly dueDate,
        Hour dueHour)
    {
        var data = new CreateClassActivityIn { Title = title, Description = description, Type = type, Weight = weight, DueDate = dueDate, DueHour = dueHour };

        var response = await http.PostAsJsonAsync($"/teacher/classes/{classId}/activities", data);

        return await response.Resolve<SuccessOut>();
    }
}
