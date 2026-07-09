using System.Net.Http.Json;

namespace Estud.Tests.Clients;

public class StudentHttpClient(HttpClient http)
{
    public readonly HttpClient Http = http;

    public async Task<List<EnrollmentClassOut>> GetStudentEnrollmentClasses()
    {
        return await Http.GetFromJsonAsync<List<EnrollmentClassOut>>("/student/enrollment-classes", HttpConfigs.JsonOptions) ?? [];
    }

    public async Task<List<DisciplineOut>> GetStudentDisciplines()
    {
        return await Http.GetFromJsonAsync<List<DisciplineOut>>("/student/disciplines", HttpConfigs.JsonOptions) ?? [];
    }

    public async Task<List<StudentNoteOut>> GetStudentNotes()
    {
        return await Http.GetFromJsonAsync<List<StudentNoteOut>>("/student/notes", HttpConfigs.JsonOptions) ?? [];
    }

    public async Task<EnrollmentPeriodOut> GetCurrentEnrollmentPeriod()
    {
        return await Http.GetFromJsonAsync<EnrollmentPeriodOut>("/student/enrollment-periods/current", HttpConfigs.JsonOptions) ?? new();
    }

    public async Task<StudentInsightsOut> GetStudentInsights()
    {
        return await Http.GetFromJsonAsync<StudentInsightsOut>("/student/insights", HttpConfigs.JsonOptions) ?? new();
    }

    public async Task<GetStudentFrequencyOut> GetStudentFrequency()
    {
        var response = await Http.GetAsync("/student/frequency");
        return await response.DeserializeTo<GetStudentFrequencyOut>();
    }

    public async Task<OneOf<List<GetStudentFrequenciesOut>, ErrorOut>> GetStudentFrequencies()
    {
        var response = await Http.GetAsync("/student/frequencies");
        return await response.Resolve<List<GetStudentFrequenciesOut>>();
    }

    public async Task<HttpResponseMessage> CreateStudentEnrollment(List<Guid> classes)
    {
        var data = new CreateStudentEnrollmentIn { Classes = classes };
        return await Http.PostAsJsonAsync("/student/enrollments", data);
    }

    public async Task<List<AgendaDayOut>> GetStudentAgenda()
    {
        return await Http.GetFromJsonAsync<List<AgendaDayOut>>("/student/agenda", HttpConfigs.JsonOptions) ?? [];
    }

    public async Task<GetStudentAverageNoteOut> GetStudentAverageNote()
    {
        return await Http.GetFromJsonAsync<GetStudentAverageNoteOut>("/student/average-note", HttpConfigs.JsonOptions) ?? new();
    }

    public async Task<OneOf<List<StudentClassActivityOut>, ErrorOut>> GetStudentClassActivities(Guid classId)
    {
        var response = await Http.GetAsync($"/student/classes/{classId}/activities");
        return await response.Resolve<List<StudentClassActivityOut>>();
    }

    public async Task<OneOf<ClassActivityWorkOut, ErrorOut>> CreateClassActivityWork(Guid activityId, string link)
    {
        var data = new CreateClassActivityWorkIn { Link = link };
        var response = await Http.PostAsJsonAsync($"/student/activities/{activityId}/works", data);
        return await response.Resolve<ClassActivityWorkOut>();
    }
}
