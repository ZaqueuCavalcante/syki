namespace Syki.Front.Features.Teacher.AddExamGradeNote;

public class AddExamGradeNoteClient(HttpClient http) : ITeacherClient
{
    public async Task<HttpResponseMessage> Add(Guid id, AddExamGradeNoteIn data)
    {
        return await http.PutAsJsonAsync($"/teacher/exam-grades/{id}", data);
    }
}
