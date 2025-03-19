namespace Syki.Front.Features.Teacher.AddClassActivityNote;

public class AddClassActivityNoteClient(HttpClient http) : ITeacherClient
{
    public async Task<HttpResponseMessage> Add(Guid id, AddClassActivityNoteIn data)
    {
        return await http.PutAsJsonAsync($"/teacher/notes/{id}", data);
    }
}
