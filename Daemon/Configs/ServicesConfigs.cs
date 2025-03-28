using Syki.Back.Emails;
using Syki.Back.Configs;
using Syki.Daemon.Events;
using Syki.Back.Features.Cross.CreateUser;
using Syki.Back.Features.Cross.ResetPassword;
using Syki.Back.Features.Academic.CreateClass;
using Syki.Back.Features.Academic.StartClasses;
using Syki.Back.Features.Academic.CreateTeacher;
using Syki.Back.Features.Academic.CreateStudent;
using Syki.Back.Features.Teacher.AddClassActivityNote;
using Syki.Back.Features.Academic.FinalizeClasses;
using Syki.Back.Features.Cross.SendResetPasswordToken;
using Syki.Back.Features.Teacher.CreateLessonAttendance;
using Syki.Back.Features.Student.CreateStudentEnrollment;
using Syki.Back.Features.Academic.CreateEnrollmentPeriod;
using Syki.Back.Features.Academic.UpdateEnrollmentPeriod;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Syki.Back.Features.Academic.ReleaseClassesForEnrollment;

namespace Syki.Daemon.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this IServiceCollection services)
    {
        services.AddEfCoreConfigs();

        services.AddScoped<CreateUserService>();
        services.AddScoped<CreateTeacherService>();
        services.AddScoped<CreateStudentService>();
        services.AddScoped<ResetPasswordService>();
        services.AddScoped<CreateClassService>();
        services.AddScoped<CreateEnrollmentPeriodService>();
        services.AddScoped<CreateStudentEnrollmentService>();
        services.AddScoped<SendResetPasswordTokenService>();
        services.AddScoped<ReleaseClassesForEnrollmentService>();
        services.AddScoped<StartClassesService>();
        services.AddScoped<UpdateEnrollmentPeriodService>();
        services.AddScoped<CreateLessonAttendanceService>();
        services.AddScoped<AddClassActivityNoteService>();
        services.AddScoped<FinalizeClassesService>();

        services.AddScoped<IEmailsService, EmailsService>();
        if (Env.IsDevelopment() || Env.IsTesting())
        {
            services.Replace(ServiceDescriptor.Singleton<IEmailsService, FakeEmailsService>());
        }

        services.AddIdentityConfigs();
        services.AddCommandsConfigs();
        services.AddTransient<DomainEventsProcessor>();
    }
}
