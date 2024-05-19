using Syki.Front.Features.Student.GetCurrentEnrollmentPeriod;

namespace Syki.Tests.Clients;

public static class StudentClientExtensions
{
    public static async Task<EnrollmentPeriodOut> GetCurrentEnrollmentPeriod(this HttpClient http)
    {
        var client = new GetCurrentEnrollmentPeriodClient(http);
        return await client.Get();
    }
}
