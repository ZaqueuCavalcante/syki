using Syki.Front.Features.Teacher.GetTeacherClass;
using Syki.Front.Features.Teacher.GetTeacherAgenda;
using Syki.Front.Features.Teacher.GetTeacherClasses;
using Syki.Front.Features.Teacher.GetTeacherInsights;
using Syki.Front.Features.Teacher.CreateClassActivity;
using Syki.Front.Features.Teacher.AddClassActivityNote;
using Syki.Front.Features.Teacher.CreateLessonAttendance;

namespace Syki.Tests.Clients;

public class TeacherHttpClient(HttpClient http)
{
    public readonly HttpClient Cross = http;

    public async Task<HttpResponseMessage> AddClassActivityNote(Guid id, decimal note)
    {
        var client = new AddClassActivityNoteClient(Cross);
        return await client.Add(id, new(note));
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> CreateLessonAttendance(Guid id, List<Guid> presentStudents)
    {
        var client = new CreateLessonAttendanceClient(Cross);
        return await client.Create(id, presentStudents);
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> CreateClassActivity(
        Guid classId,
        string title,
        string description,
        ClassActivityType type,
        int weight,
        DateOnly dueDate,
        Hour dueHour)
    {
        var client = new CreateClassActivityClient(Cross);
        return await client.Create(classId, title, description, type, weight, dueDate, dueHour);
    }

    public async Task<TeacherInsightsOut> GetTeacherInsights()
    {
        var client = new GetTeacherInsightsClient(Cross);
        return await client.Get();
    }

    public async Task<TeacherClassOut> GetTeacherClass(Guid id)
    {
        var client = new GetTeacherClassClient(Cross);
        return await client.Get(id);
    }

    public async Task<List<TeacherClassesOut>> GetTeacherClasses()
    {
        var client = new GetTeacherClassesClient(Cross);
        return await client.Get();
    }

    public async Task<List<AgendaDayOut>> GetTeacherAgenda()
    {
        var client = new GetTeacherAgendaClient(Cross);
        return await client.Get();
    }

    public async Task AddClassActivityNotes(Guid classId, Guid studentId, decimal n1, decimal n2, decimal n3)
    {
        var discreteMathClass = await GetTeacherClass(classId);
        var discreteMathClassNotes = discreteMathClass.Students.First(x => x.Id == studentId).Notes;
        await AddClassActivityNote(discreteMathClassNotes[0].Id, n1);
        await AddClassActivityNote(discreteMathClassNotes[1].Id, n2);
        await AddClassActivityNote(discreteMathClassNotes[2].Id, n3);
    }
}
