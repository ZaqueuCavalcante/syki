using System.Net.Http.Json;

namespace Syki.Tests.Clients;

public class TeacherHttpClient(HttpClient http)
{
    public readonly HttpClient Cross = http;

    public async Task<OneOf<SuccessOut, ErrorOut>> CreateLessonAttendance(Guid id, List<Guid> presentStudents)
    {
        var data = new CreateLessonAttendanceIn() { PresentStudents = presentStudents };
        var response = await Cross.PutAsJsonAsync($"/teacher/lessons/{id}/attendance", data);
        return await response.Resolve<SuccessOut>();
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

        var data = new CreateClassActivityIn { Note = note, Title = title, Description = description, Type = type, Weight = weight, DueDate = dueDate, DueHour = dueHour };
        var response = await Cross.PostAsJsonAsync($"/teacher/classes/{classId}/activities", data);
        return await response.Resolve<CreateClassActivityOut>();
    }

    public async Task<TeacherInsightsOut> GetTeacherInsights()
    {
        return await Cross.GetFromJsonAsync<TeacherInsightsOut>("/teacher/insights", HttpConfigs.JsonOptions) ?? new();
    }

    public async Task<OneOf<TeacherClassOut, ErrorOut>> GetTeacherClass(Guid id)
    {
        var response = await Cross.GetAsync($"/teacher/classes/{id}");
        return await response.Resolve<TeacherClassOut>();
    }

    public async Task<OneOf<List<TeacherClassStudentOut>, ErrorOut>> GetTeacherClassStudents(Guid classId)
    {
        var response = await Cross.GetAsync($"teacher/classes/{classId}/students");
        return await response.Resolve<List<TeacherClassStudentOut>>();
    }

    public async Task<OneOf<List<ClassNoteRemainingWeightsOut>, ErrorOut>> GetClassNotesRemainingWeights(Guid classId)
    {
        var response = await Cross.GetAsync($"/teacher/classes/{classId}/remaining-weights");
        return await response.Resolve<List<ClassNoteRemainingWeightsOut>>();
    }

    public async Task<OneOf<TeacherClassActivityOut, ErrorOut>> GetTeacherClassActivity(Guid classId, Guid activityId)
    {
        var response = await Cross.GetAsync($"teacher/classes/{classId}/activities/{activityId}");
        return await response.Resolve<TeacherClassActivityOut>();
    }

    public async Task<OneOf<List<TeacherClassActivityOut>, ErrorOut>> GetTeacherClassActivities(Guid classId)
    {
        var response = await Cross.GetAsync($"teacher/classes/{classId}/activities");
        return await response.Resolve<List<TeacherClassActivityOut>>();
    }

    public async Task<OneOf<List<ClassLessonOut>, ErrorOut>> GetTeacherClassLessons(Guid classId)
    {
        var response = await Cross.GetAsync($"teacher/classes/{classId}/lessons");
        return await response.Resolve<List<ClassLessonOut>>();
    }

    public async Task<List<TeacherClassesOut>> GetTeacherClasses()
    {
        return await Cross.GetFromJsonAsync<List<TeacherClassesOut>>("/teacher/classes", HttpConfigs.JsonOptions) ?? [];
    }

    public async Task<List<AgendaDayOut>> GetTeacherAgenda()
    {
        return await Cross.GetFromJsonAsync<List<AgendaDayOut>>("/teacher/agenda", HttpConfigs.JsonOptions) ?? [];
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> AddStudentClassActivityNote(Guid classActivityId, Guid studentId, decimal value)
    {
        var data = new AddStudentClassActivityNoteIn(studentId, value);
        var response = await Cross.PostAsJsonAsync($"teacher/class-activities/{classActivityId}", data);
        return await response.Resolve<SuccessOut>();
    }

    public async Task AddClassActivityNote(Guid classId, Guid studentId, ClassNoteType type, int weight, decimal note)
    {
        CreateClassActivityOut activity = await CreateClassActivity(classId, note: type, weight: weight);
        await AddStudentClassActivityNote(activity.Id, studentId, note);
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> SetSchedulingPreferences(List<ScheduleIn> schedules)
    {
        var data = new SetSchedulingPreferencesIn { Schedules = schedules };
        var response = await Cross.PutAsJsonAsync($"teacher/scheduling-preferences", data);
        return await response.Resolve<SuccessOut>();
    }
}
