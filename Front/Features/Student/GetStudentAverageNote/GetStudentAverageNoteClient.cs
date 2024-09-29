namespace Syki.Front.Features.Student.GetStudentAverageNote;

public class GetStudentAverageNoteClient(HttpClient http) : IStudentClient
{
    public async Task<GetStudentAverageNoteOut> Get()
    {
        return await http.GetFromJsonAsync<GetStudentAverageNoteOut>("/student/average-note") ?? new();
    }
}
