using Syki.Front.Features.Student.GetStudentAgenda;
using Syki.Front.Features.Student.GetStudentInsights;
using Syki.Front.Features.Student.GetStudentDisciplines;
using Syki.Front.Features.Student.CreateStudentEnrollment;
using Syki.Front.Features.Student.GetCurrentEnrollmentPeriod;
using Syki.Front.Features.Student.GetStudentEnrollmentClasses;
using Syki.Front.Features.Student.GetStudentExamGrades;

namespace Syki.Tests.Clients;

public static class StudentClientExtensions
{
    public static async Task<List<EnrollmentClassOut>> GetStudentEnrollmentClasses(this HttpClient http)
    {
        var client = new GetStudentEnrollmentClassesClient(http);
        return await client.Get();
    }

    public static async Task<List<DisciplineOut>> GetStudentDisciplines(this HttpClient http)
    {
        var client = new GetStudentDisciplinesClient(http);
        return await client.Get();
    }

    public static async Task<List<StudentExamGradeOut>> GetStudentExamGrades(this HttpClient http)
    {
        var client = new GetStudentExamGradesClient(http);
        return await client.Get();
    }

    public static async Task<EnrollmentPeriodOut> GetCurrentEnrollmentPeriod(this HttpClient http)
    {
        var client = new GetCurrentEnrollmentPeriodClient(http);
        return await client.Get();
    }

    public static async Task<StudentInsightsOut> GetStudentInsights(this HttpClient http)
    {
        var client = new GetStudentInsightsClient(http);
        return await client.Get();
    }

    public static async Task<HttpResponseMessage> CreateStudentEnrollment(this HttpClient http, List<Guid> classes)
    {
        var client = new CreateStudentEnrollmentClient(http);
        return await client.Create(classes);
    }

    public static async Task<List<AgendaDayOut>> GetStudentAgenda(this HttpClient http)
    {
        var client = new GetStudentAgendaClient(http);
        return await client.Get();
    }
}
