namespace Syki.Front.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this WebAssemblyHostBuilder builder)
    {
        // Academic
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
        builder.Services.AddScoped<UpdateCampusClient>();

        // Adm
        builder.Services.AddScoped<GetAdmInsightsClient>();
        builder.Services.AddScoped<GetInstitutionsClient>();
        builder.Services.AddScoped<GetUsersClient>();

        // Cross
        builder.Services.AddScoped<CreatePendingUserRegisterClient>();
        builder.Services.AddScoped<FinishUserRegisterClient>();
        builder.Services.AddScoped<GetMfaKeyClient>();
        builder.Services.AddScoped<GetUserNotificationsClient>();
        builder.Services.AddScoped<LoginClient>();
        builder.Services.AddScoped<LoginMfaClient>();
        builder.Services.AddScoped<ResetPasswordClient>();
        builder.Services.AddScoped<SendResetPasswordTokenClient>();
        builder.Services.AddScoped<SetupMfaClient>();
        builder.Services.AddScoped<ViewNotificationsClient>();

        // Student
        builder.Services.AddScoped<CreateStudentEnrollmentClient>();
        builder.Services.AddScoped<GetCurrentEnrollmentPeriodClient>();
        builder.Services.AddScoped<GetStudentAgendaClient>();
        builder.Services.AddScoped<GetStudentDisciplinesClient>();
        builder.Services.AddScoped<GetStudentEnrollmentClassesClient>();
        builder.Services.AddScoped<GetStudentInsightsClient>();

        // Teacher
        builder.Services.AddScoped<GetTeacherAgendaClient>();
        builder.Services.AddScoped<GetTeacherClassesClient>();
        builder.Services.AddScoped<GetTeacherInsightsClient>();

        // Seller
        builder.Services.AddScoped<GetSellerCourseOfferingsClient>();
    }
}
