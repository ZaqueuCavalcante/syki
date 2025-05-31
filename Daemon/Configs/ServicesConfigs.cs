using Syki.Back.Emails;
using Syki.Back.Configs;
using Syki.Daemon.Events;
using Syki.Back.Features.Cross.CreateUser;
using Syki.Back.Features.Cross.ResetPassword;
using Syki.Back.Features.Academic.CreateClass;
using Syki.Back.Features.Academic.StartClasses;
using Syki.Back.Features.Academic.CreateTeacher;
using Syki.Back.Features.Academic.CreateStudent;
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
    public static void AddServicesConfigs(this WebApplicationBuilder builder)
    {
        builder.AddEfCoreConfigs();

        builder.Services.AddScoped<CreateUserService>();
        builder.Services.AddScoped<CreateTeacherService>();
        builder.Services.AddScoped<CreateStudentService>();
        builder.Services.AddScoped<ResetPasswordService>();
        builder.Services.AddScoped<CreateClassService>();
        builder.Services.AddScoped<CreateEnrollmentPeriodService>();
        builder.Services.AddScoped<CreateStudentEnrollmentService>();
        builder.Services.AddScoped<SendResetPasswordTokenService>();
        builder.Services.AddScoped<ReleaseClassesForEnrollmentService>();
        builder.Services.AddScoped<StartClassesService>();
        builder.Services.AddScoped<UpdateEnrollmentPeriodService>();
        builder.Services.AddScoped<CreateLessonAttendanceService>();
        builder.Services.AddScoped<FinalizeClassesService>();

        builder.Services.AddScoped<IEmailsService, EmailsService>();
        if (Env.IsDevelopment() || Env.IsTesting())
        {
            builder.Services.Replace(ServiceDescriptor.Singleton<IEmailsService, FakeEmailsService>());
        }

        builder.AddIdentityConfigs();
        builder.AddCommandsConfigs();
        builder.Services.AddTransient<DomainEventsProcessor>();

        builder.Services.AddHttpClient();
    }
}
