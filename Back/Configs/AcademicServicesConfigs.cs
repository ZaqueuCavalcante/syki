using Syki.Back.Features.Academic.GetCampi;
using Syki.Back.Features.Academic.UpdateCampus;
using Syki.Back.Features.Academic.CreateCampus;
using Syki.Back.Features.Academic.CreateDiscipline;
using Syki.Back.Features.Academic.GetEnrollmentPeriods;
using Syki.Back.Features.Academic.CreateAcademicPeriod;
using Syki.Back.Features.Academic.CreateTeacher;
using Syki.Back.Features.Academic.CreateEnrollmentPeriod;
using Syki.Back.Features.Academic.CreateNotification;
using Syki.Back.Features.Academic.CreateCourseOffering;
using Syki.Back.Features.Academic.CreateClass;
using Syki.Back.Features.Academic.GetAcademicPeriods;
using Syki.Back.Features.Academic.GetStudents;
using Syki.Back.Features.Academic.GetCoursesWithDisciplines;
using Syki.Back.Features.Academic.GetDisciplines;
using Syki.Back.Features.Academic.GetNotifications;
using Syki.Back.Features.Academic.GetTeachers;
using Syki.Back.Features.Academic.GetClasses;
using Syki.Back.Features.Academic.GetCoursesWithCurriculums;
using Syki.Back.Features.Academic.CreateStudent;
using Syki.Back.Features.Academic.CreateCourse;
using Syki.Back.Features.Academic.GetAcademicInsights;
using Syki.Back.Features.Academic.GetCourses;
using Syki.Back.Features.Academic.CreateCourseCurriculum;
using Syki.Back.Features.Academic.GetCourseDisciplines;
using Syki.Back.Features.Academic.GetCourseOfferings;
using Syki.Back.Features.Academic.GetCourseCurriculums;

namespace Syki.Back.Configs;

public static class AcademicServicesConfigs
{
    public static void AddAcademicServicesConfigs(this IServiceCollection services)
    {
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
    }
}
