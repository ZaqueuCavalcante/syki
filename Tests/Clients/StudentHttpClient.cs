using Syki.Front.Features.Student.GetStudentNotes;
using Syki.Front.Features.Student.GetStudentAgenda;
using Syki.Front.Features.Student.GetStudentInsights;
using Syki.Front.Features.Student.GetStudentFrequency;
using Syki.Front.Features.Student.GetStudentAverageNote;
using Syki.Front.Features.Student.GetStudentDisciplines;
using Syki.Front.Features.Student.GetStudentFrequencies;
using Syki.Front.Features.Student.CreateStudentEnrollment;
using Syki.Front.Features.Student.CreateClassActivityWork;
using Syki.Front.Features.Student.GetStudentClassActivities;
using Syki.Front.Features.Student.GetCurrentEnrollmentPeriod;
using Syki.Front.Features.Student.GetStudentEnrollmentClasses;

namespace Syki.Tests.Clients;

public class StudentHttpClient(HttpClient http)
{
    public readonly HttpClient Http = http;

    public async Task<List<EnrollmentClassOut>> GetStudentEnrollmentClasses()
    {
        var client = new GetStudentEnrollmentClassesClient(Http);
        return await client.Get();
    }

    public async Task<List<DisciplineOut>> GetStudentDisciplines()
    {
        var client = new GetStudentDisciplinesClient(Http);
        return await client.Get();
    }

    public async Task<List<StudentNoteOut>> GetStudentNotes()
    {
        var client = new GetStudentNotesClient(Http);
        return await client.Get();
    }

    public async Task<EnrollmentPeriodOut> GetCurrentEnrollmentPeriod()
    {
        var client = new GetCurrentEnrollmentPeriodClient(Http);
        return await client.Get();
    }

    public async Task<StudentInsightsOut> GetStudentInsights()
    {
        var client = new GetStudentInsightsClient(Http);
        return await client.Get();
    }

    public async Task<GetStudentFrequencyOut> GetStudentFrequency()
    {
        var client = new GetStudentFrequencyClient(Http);
        return await client.Get();
    }

    public async Task<OneOf<List<GetStudentFrequenciesOut>, ErrorOut>> GetStudentFrequencies()
    {
        var client = new GetStudentFrequenciesClient(Http);
        return await client.Get();
    }

    public async Task<HttpResponseMessage> CreateStudentEnrollment(List<Guid> classes)
    {
        var client = new CreateStudentEnrollmentClient(Http);
        return await client.Create(classes);
    }

    public async Task<List<AgendaDayOut>> GetStudentAgenda()
    {
        var client = new GetStudentAgendaClient(Http);
        return await client.Get();
    }

    public async Task<GetStudentAverageNoteOut> GetStudentAverageNote()
    {
        var client = new GetStudentAverageNoteClient(Http);
        return await client.Get();
    }

    public async Task<OneOf<List<StudentClassActivityOut>, ErrorOut>> GetStudentClassActivities(Guid classId)
    {
        var client = new GetStudentClassActivitiesClient(Http);
        return await client.Get(classId);
    }

    public async Task<OneOf<ClassActivityWorkOut, ErrorOut>> CreateClassActivityWork(Guid activityId, string link)
    {
        var client = new CreateClassActivityWorkClient(Http);
        return await client.Create(activityId, link);
    }
}
