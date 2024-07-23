using Syki.Front.Features.Teacher.GetTeacherClass;
using Syki.Front.Features.Teacher.GetTeacherAgenda;
using Syki.Front.Features.Teacher.GetTeacherClasses;
using Syki.Front.Features.Teacher.GetTeacherInsights;

namespace Syki.Tests.Clients;

public static class TeacherClientExtensions
{
    public static async Task<TeacherInsightsOut> GetTeacherInsights(this HttpClient http)
    {
        var client = new GetTeacherInsightsClient(http);
        return await client.Get();
    }

    public static async Task<TeacherClassOut> GetTeacherClass(this HttpClient http, Guid id)
    {
        var client = new GetTeacherClassClient(http);
        return await client.Get(id);
    }

    public static async Task<List<TeacherClassesOut>> GetTeacherClasses(this HttpClient http)
    {
        var client = new GetTeacherClassesClient(http);
        return await client.Get();
    }

    public static async Task<List<AgendaDayOut>> GetTeacherAgenda(this HttpClient http)
    {
        var client = new GetTeacherAgendaClient(http);
        return await client.Get();
    }
}
