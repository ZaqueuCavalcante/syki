using Syki.Back.Features.Student.CreateStudentEnrollment;
using Syki.Back.Features.Student.GetStudentAgenda;
using Syki.Back.Features.Student.GetStudentDisciplines;
using Syki.Back.Features.Student.GetCurrentEnrollmentPeriod;
using Syki.Back.Features.Student.GetStudentInsights;

using Syki.Back.Features.Student.GetStudentEnrollmentClasses;

namespace Syki.Back.Configs;

public static class StudentServicesConfigs
{
    public static void AddStudentServicesConfigs(this IServiceCollection services)
    {
        services.AddScoped<CreateStudentEnrollmentService>();
        services.AddScoped<GetCurrentEnrollmentPeriodService>();
        services.AddScoped<GetStudentAgendaService>();
        services.AddScoped<GetStudentDisciplinesService>();
        services.AddScoped<GetStudentEnrollmentClassesService>();
        services.AddScoped<GetStudentInsightsService>();
    }
}
