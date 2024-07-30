using Syki.Front.Features.Teacher.AddExamGradeNote;
using Syki.Front.Features.Teacher.GetTeacherClass;
using Syki.Front.Features.Teacher.GetTeacherAgenda;
using Syki.Front.Features.Teacher.GetTeacherClasses;
using Syki.Front.Features.Teacher.GetTeacherInsights;

namespace Syki.Tests.Clients;

public class TeacherHttpClient(HttpClient http)
{
    public readonly HttpClient Cross = http;

    public async Task<HttpResponseMessage> AddExamGradeNote(Guid id, decimal note)
    {
        var client = new AddExamGradeNoteClient(Cross);
        return await client.Add(id, new(note));
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
}
