using System.Net.Http.Json;
using Syki.Front.Features.Teacher.AddStudentClassActivityNote;
using Syki.Front.Features.Teacher.GetTeacherClass;
using Syki.Front.Features.Teacher.GetTeacherAgenda;
using Syki.Front.Features.Teacher.GetTeacherClasses;
using Syki.Front.Features.Teacher.GetTeacherInsights;
using Syki.Front.Features.Teacher.CreateClassActivity;
using Syki.Front.Features.Teacher.CreateLessonAttendance;
using Syki.Front.Features.Teacher.GetTeacherClassLessons;
using Syki.Front.Features.Teacher.GetTeacherClassActivities;
using Syki.Front.Features.Teacher.GetTeacherClassActivity;
using Syki.Front.Features.Teacher.GetClassNotesRemainingWeights;

namespace Syki.Tests.Clients;

public class TeacherHttpClient(HttpClient http)
{
    public readonly HttpClient Cross = http;

    public async Task<OneOf<SuccessOut, ErrorOut>> CreateLessonAttendance(Guid id, List<Guid> presentStudents)
    {
        var client = new CreateLessonAttendanceClient(Cross);
        return await client.Create(id, presentStudents);
    }

    public async Task<OneOf<CreateClassActivityOut, ErrorOut>> CreateClassActivity(
        Guid classId,
        ClassNoteType note = ClassNoteType.N1,
        string title = "Modelagem de Banco de Dados",
        string description = "Modele um banco de dados para uma barbearia.",
        ClassActivityType type = ClassActivityType.Work,
        int weight = 25,
        DateOnly dueDate = default,
        Hour dueHour = Hour.H12_00)
    {
        dueDate = dueDate == default ? DateTime.UtcNow.AddDays(15).ToDateOnly() : dueDate;
        var client = new CreateClassActivityClient(Cross);
        return await client.Create(classId, note, title, description, type, weight, dueDate, dueHour);
    }

    public async Task<TeacherInsightsOut> GetTeacherInsights()
    {
        var client = new GetTeacherInsightsClient(Cross);
        return await client.Get();
    }

    public async Task<OneOf<TeacherClassOut, ErrorOut>> GetTeacherClass(Guid id)
    {
        var client = new GetTeacherClassClient(Cross);
        return await client.Get(id);
    }

    public async Task<OneOf<List<ClassNoteRemainingWeightsOut>, ErrorOut>> GetClassNotesRemainingWeights(Guid classId)
    {
        var client = new GetClassNotesRemainingWeightsClient(Cross);
        return await client.Get(classId);
    }

    public async Task<TeacherClassActivityOut> GetTeacherClassActivity(Guid classId, Guid activityId)
    {
        var client = new GetTeacherClassActivityClient(Cross);
        return await client.Get(classId, activityId);
    }

    public async Task<OneOf<List<TeacherClassActivityOut>, ErrorOut>> GetTeacherClassActivities(Guid classId)
    {
        var client = new GetTeacherClassActivitiesClient(Cross);
        return await client.Get(classId);
    }

    public async Task<OneOf<List<ClassLessonOut>, ErrorOut>> GetTeacherClassLessons(Guid classId)
    {
        var client = new GetTeacherClassLessonsClient(Cross);
        return await client.Get(classId);
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

    public async Task<OneOf<SuccessOut, ErrorOut>> AddStudentClassActivityNote(Guid classActivityId, Guid studentId, decimal value)
    {
        var client = new AddStudentClassActivityNoteClient(Cross);

        return await client.Add(classActivityId, studentId, value);
    }

    public async Task AddClassActivityNotes(Guid classId, Guid studentId, decimal n1, decimal n2, decimal n3)
    {
        var discreteMathClass = await GetTeacherClass(classId);
    }
}
