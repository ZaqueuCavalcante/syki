using Syki.Front.Features.Teacher.GetTeacherInsights;

namespace Syki.Tests.Clients;

public static class TeacherClientExtensions
{
    public static async Task<TeacherInsightsOut> GetTeacherInsights(this HttpClient http)
    {
        var client = new GetTeacherInsightsClient(http);
        return await client.Get();
    }
}
