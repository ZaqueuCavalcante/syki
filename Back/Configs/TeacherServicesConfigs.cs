using Syki.Back.Features.Teacher.AddExamGradeNote;
using Syki.Back.Features.Teacher.GetTeacherClass;
using Syki.Back.Features.Teacher.GetTeacherAgenda;
using Syki.Back.Features.Teacher.GetTeacherClasses;
using Syki.Back.Features.Teacher.GetTeacherInsights;

namespace Syki.Back.Configs;

public static class TeacherServicesConfigs
{
    public static void AddTeacherServicesConfigs(this IServiceCollection services)
    {
        services.AddScoped<GetTeacherAgendaService>();
        services.AddScoped<GetTeacherClassService>();
        services.AddScoped<GetTeacherClassesService>();
        services.AddScoped<GetTeacherInsightsService>();
        services.AddScoped<AddExamGradeNoteService>();
    }
}
