using Syki.Back.Features.Teacher.GetTeacherAgenda;
using Syki.Back.Features.Teacher.GetTeacherInsights;
using Syki.Back.Features.Teacher.GetTeacherClasses;
using Syki.Back.Features.Teacher.GetTeacherClass;

namespace Syki.Back.Configs;

public static class TeacherServicesConfigs
{
    public static void AddTeacherServicesConfigs(this IServiceCollection services)
    {
        services.AddScoped<GetTeacherAgendaService>();
        services.AddScoped<GetTeacherClassService>();
        services.AddScoped<GetTeacherClassesService>();
        services.AddScoped<GetTeacherInsightsService>();
    }
}
