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

    public static async Task<List<TeacherClassOut>> GetTeacherClasses(this HttpClient http)
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
