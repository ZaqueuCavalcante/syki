namespace Syki.Front.Features.Student.GetStudentNotes;

public class GetStudentNotesClient(HttpClient http) : IStudentClient
{
    public async Task<List<StudentNoteOut>> Get()
    {
        return await http.GetFromJsonAsync<List<StudentNoteOut>>("/student/notes", HttpConfigs.JsonOptions) ?? [];
    }
}
