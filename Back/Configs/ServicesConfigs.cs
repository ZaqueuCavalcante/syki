using Syki.Back.Features.Academic.GetCampi;
using Syki.Back.Features.Academic.UpdateCampus;
using Syki.Back.Features.Academic.CreateCampus;
using Syki.Back.Features.Academic.CreateDiscipline;
using Syki.Back.Features.Academic.GetEnrollmentPeriods;
using Syki.Back.Features.Academic.CreateAcademicPeriod;
using Syki.Back.Features.Academic.CreateTeacher;
using Syki.Back.Features.Academic.CreateEnrollmentPeriod;
using Syki.Back.Features.Student.CreateStudentEnrollment;
using Syki.Back.Features.Academic.CreateNotification;
using Syki.Back.Features.Academic.CreateCourseOffering;
using Syki.Back.Features.Academic.CreateClass;
using Syki.Back.Features.Academic.GetAcademicPeriods;
using Syki.Back.Features.Student.GetStudentAgenda;
using Syki.Back.Features.Student.GetStudentDisciplines;
using Syki.Back.Features.Academic.GetStudents;
using Syki.Back.Features.Student.GetCurrentEnrollmentPeriod;
using Syki.Back.Features.Academic.GetCoursesWithDisciplines;
using Syki.Back.Features.Academic.GetDisciplines;
using Syki.Back.Features.Adm.GetInstitutions;
using Syki.Back.Features.Academic.GetNotifications;
using Syki.Back.Features.Academic.GetTeachers;
using Syki.Back.Features.Adm.GetAdmInsights;
using Syki.Back.Features.Academic.GetClasses;
using Syki.Back.Features.Adm.GetUsers;
using Syki.Back.Features.Academic.GetCoursesWithCurriculums;
using Syki.Back.Features.Teacher.GetTeacherAgenda;
using Syki.Back.Features.Cross.Login;
using Syki.Back.Features.Cross.SetupMfa;
using Syki.Back.Features.Cross.LoginMfa;
using Syki.Back.Features.Cross.GetMfaKey;
using Syki.Back.Features.Cross.GenerateJWT;
using Syki.Back.Features.Cross.ResetPassword;
using Syki.Back.Features.Cross.ViewNotifications;
using Syki.Back.Features.Cross.FinishUserRegister;
using Syki.Back.Features.Cross.GetUserNotifications;
using Syki.Back.Features.Cross.SendResetPasswordToken;
using Syki.Back.Features.Cross.CreatePendingUserRegister;
using Syki.Back.Features.Academic.CreateStudent;
using Syki.Back.Features.Cross.CreateUser;
using Syki.Back.Features.Academic.CreateCourse;
using Syki.Back.Features.Academic.GetAcademicInsights;
using Syki.Back.Features.Academic.GetCourses;
using Syki.Back.Features.Academic.CreateCourseCurriculum;
using Syki.Back.Features.Student.GetStudentInsights;
using Syki.Back.Features.Academic.GetCourseDisciplines;
using Syki.Back.Features.Teacher.GetTeacherInsights;
using Syki.Back.Features.Teacher.GetTeacherClasses;
using Syki.Back.Features.Teacher.GetTeacherClass;
using Syki.Back.Features.Academic.GetCourseOfferings;
using Syki.Back.Features.Academic.GetCourseCurriculums;
using Syki.Back.Features.Student.GetStudentEnrollmentClasses;

namespace Syki.Back.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this IServiceCollection services)
    {
        // Academic
        services.AddScoped<CreateAcademicPeriodService>();
        services.AddScoped<CreateCampusService>();
        services.AddScoped<CreateClassService>();
        services.AddScoped<CreateCourseService>();
        services.AddScoped<CreateCourseCurriculumService>();
        services.AddScoped<CreateCourseOfferingService>();
        services.AddScoped<CreateDisciplineService>();
        services.AddScoped<CreateEnrollmentPeriodService>();
        services.AddScoped<CreateNotificationService>();
        services.AddScoped<CreateStudentService>();
        services.AddScoped<CreateTeacherService>();
        services.AddScoped<GetAcademicInsightsService>();
        services.AddScoped<GetAcademicPeriodsService>();
        services.AddScoped<GetCampiService>();
        services.AddScoped<GetClassesService>();
        services.AddScoped<GetCourseCurriculumsService>();
        services.AddScoped<GetCourseDisciplinesService>();
        services.AddScoped<GetCourseOfferingsService>();
        services.AddScoped<GetCoursesService>();
        services.AddScoped<GetCoursesWithCurriculumsService>();
        services.AddScoped<GetCoursesWithDisciplinesService>();
        services.AddScoped<GetDisciplinesService>();
        services.AddScoped<GetEnrollmentPeriodsService>();
        services.AddScoped<GetNotificationsService>();
        services.AddScoped<GetStudentsService>();
        services.AddScoped<GetTeachersService>();
        services.AddScoped<UpdateCampusService>();

        // Adm
        services.AddScoped<GetAdmInsightsService>();
        services.AddScoped<GetInstitutionsService>();
        services.AddScoped<GetUsersService>();

        // Cross
        services.AddScoped<CreatePendingUserRegisterService>();
        services.AddScoped<CreateUserService>();
        services.AddScoped<FinishUserRegisterService>();
        services.AddScoped<GenerateJWTService>();
        services.AddScoped<GetMfaKeyService>();
        services.AddScoped<GetUserNotificationsService>();
        services.AddScoped<LoginService>();
        services.AddScoped<LoginMfaService>();
        services.AddScoped<ResetPasswordService>();
        services.AddScoped<SendResetPasswordTokenService>();
        services.AddScoped<SetupMfaService>();
        services.AddScoped<ViewNotificationsService>();

        // Student
        services.AddScoped<CreateStudentEnrollmentService>();
        services.AddScoped<GetCurrentEnrollmentPeriodService>();
        services.AddScoped<GetStudentAgendaService>();
        services.AddScoped<GetStudentDisciplinesService>();
        services.AddScoped<GetStudentEnrollmentClassesService>();
        services.AddScoped<GetStudentInsightsService>();

        // Teacher
        services.AddScoped<GetTeacherAgendaService>();
        services.AddScoped<GetTeacherClassService>();
        services.AddScoped<GetTeacherClassesService>();
        services.AddScoped<GetTeacherInsightsService>();
    }
}
