namespace Syki.Front.Configs;

public static class AcademicServicesConfigs
{
    public static void AddAcademicServicesConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<CreateAcademicPeriodClient>();
        builder.Services.AddScoped<CreateCampusClient>();
        builder.Services.AddScoped<CreateClassClient>();
        builder.Services.AddScoped<CreateCourseClient>();
        builder.Services.AddScoped<CreateCourseCurriculumClient>();
        builder.Services.AddScoped<CreateCourseOfferingClient>();
        builder.Services.AddScoped<CreateDisciplineClient>();
        builder.Services.AddScoped<CreateEnrollmentPeriodClient>();
        builder.Services.AddScoped<CreateNotificationClient>();
        builder.Services.AddScoped<CreateStudentClient>();
        builder.Services.AddScoped<CreateTeacherClient>();
        builder.Services.AddScoped<GetAcademicInsightsClient>();
        builder.Services.AddScoped<GetAcademicPeriodsClient>();
        builder.Services.AddScoped<GetCampiClient>();
        builder.Services.AddScoped<GetClassesClient>();
        builder.Services.AddScoped<GetCourseCurriculumsClient>();
        builder.Services.AddScoped<GetCourseDisciplinesClient>();
        builder.Services.AddScoped<GetCourseOfferingsClient>();
        builder.Services.AddScoped<GetCoursesClient>();
        builder.Services.AddScoped<GetCoursesWithCurriculumsClient>();
        builder.Services.AddScoped<GetCoursesWithDisciplinesClient>();
        builder.Services.AddScoped<GetDisciplinesClient>();
        builder.Services.AddScoped<GetEnrollmentPeriodsClient>();
        builder.Services.AddScoped<GetNotificationsClient>();
        builder.Services.AddScoped<GetStudentsClient>();
        builder.Services.AddScoped<GetTeachersClient>();
        builder.Services.AddScoped<StartClassClient>();
        builder.Services.AddScoped<UpdateCampusClient>();
    }
}
