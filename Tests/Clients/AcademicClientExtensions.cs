using Syki.Front.Features.Academic.GetCampi;
using Syki.Front.Features.Academic.GetCourses;
using Syki.Front.Features.Academic.CreateClass;
using Syki.Front.Features.Academic.GetTeachers;
using Syki.Front.Features.Academic.CreateCampus;
using Syki.Front.Features.Academic.UpdateCampus;
using Syki.Front.Features.Academic.CreateCourse;
using Syki.Front.Features.Academic.CreateStudent;
using Syki.Front.Features.Academic.CreateTeacher;
using Syki.Front.Features.Academic.GetDisciplines;
using Syki.Front.Features.Academic.CreateDiscipline;
using Syki.Front.Features.Academic.GetCourseOfferings;
using Syki.Front.Features.Academic.GetAcademicPeriods;
using Syki.Front.Features.Academic.CreateNotification;
using Syki.Front.Features.Academic.GetAcademicInsights;
using Syki.Front.Features.Academic.CreateAcademicPeriod;
using Syki.Front.Features.Academic.GetCourseDisciplines;
using Syki.Front.Features.Academic.CreateCourseOffering;
using Syki.Front.Features.Academic.GetEnrollmentPeriods;
using Syki.Front.Features.Academic.GetCourseCurriculums;
using Syki.Front.Features.Academic.CreateEnrollmentPeriod;
using Syki.Front.Features.Academic.CreateCourseCurriculum;
using Syki.Front.Features.Academic.GetClasses;

namespace Syki.Tests.Clients;

public static class AcademicClientExtensions
{
    public static async Task<NotificationOut> CreateNotification(this HttpClient http, string title, string description, UsersGroup targetUsers)
    {
        var client = new CreateNotificationClient(http);
        var response = await client.Create(title, description, targetUsers);
        return await response.DeserializeTo<NotificationOut>();
    }

    public static async Task<CampusOut> CreateCampus(this HttpClient http, string name = "Agreste I", string city = "Caruaru - PE")
    {
        var client = new CreateCampusClient(http);
        var response = await client.Create(name, city);
        return await response.DeserializeTo<CampusOut>();
    }

    public static async Task<List<CampusOut>> GetCampi(this HttpClient http)
    {
        var client = new GetCampiClient(http);
        return await client.Get();
    }

    public static async Task<DisciplineOut> CreateDiscipline(this HttpClient http, string name = "Banco de Dados", List<Guid> courses = null)
    {
        var client = new CreateDisciplineClient(http);
        var response = await client.Create(name, courses ?? []);
        return await response.DeserializeTo<DisciplineOut>();
    }

    public static async Task<List<DisciplineOut>> GetDisciplines(this HttpClient http)
    {
        var client = new GetDisciplinesClient(http);
        return await client.Get();
    }

    public static async Task<List<CourseDisciplineOut>> GetCourseDisciplines(this HttpClient http, Guid courseId)
    {
        var client = new GetCourseDisciplinesClient(http);
        return await client.Get(courseId);
    }

    public static async Task<CampusOut> UpdateCampus(this HttpClient http, Guid id, string name = "Agreste I", string city = "Caruaru - PE")
    {
        var client = new UpdateCampusClient(http);
        var response = await client.Update(id, name, city);
        return await response.DeserializeTo<CampusOut>();
    }

    public static async Task<CourseOut> CreateCourse(this HttpClient http, string name = "ADS", CourseType tipo = CourseType.Bacharelado)
    {
        var client = new CreateCourseClient(http);
        var response = await client.Create(name, tipo);
        return await response.DeserializeTo<CourseOut>();
    }

    public static async Task<List<CourseOut>> GetCourses(this HttpClient http)
    {
        var client = new GetCoursesClient(http);
        return await client.Get();
    }

    public static async Task<HttpResponseMessage> CreateCourseCurriculumHttp(
        this HttpClient http,
        string name,
        Guid courseId,
        List<CreateCourseCurriculumDisciplineIn> disciplines = null
    ) {
        var client = new CreateCourseCurriculumClient(http);
        return await client.Create(name, courseId, disciplines ?? []);
    }

    public static async Task<CourseCurriculumOut> CreateCourseCurriculum(
        this HttpClient http,
        string name,
        Guid courseId,
        List<CreateCourseCurriculumDisciplineIn> disciplines = null
    ) {
        var result = await http.CreateCourseCurriculumHttp(name, courseId, disciplines ?? []);
        return await result.DeserializeTo<CourseCurriculumOut>();
    }

    public static async Task<List<CourseCurriculumOut>> GetCourseCurriculums(this HttpClient http)
    {
        var client = new GetCourseCurriculumsClient(http);
        return await client.Get();
    }

    public static async Task<HttpResponseMessage> CreateCourseOfferingHttp(
        this HttpClient http,
        Guid campusId,
        Guid courseId,
        Guid courseCurriculumId,
        string? period,
        Shift shift
    ) {
        var client = new CreateCourseOfferingClient(http);

        return await client.Create(campusId, courseId, courseCurriculumId, period, shift);
    }

    public static async Task<CourseOfferingOut> CreateCourseOffering(
        this HttpClient http,
        Guid campusId,
        Guid courseId,
        Guid courseCurriculumId,
        string? period,
        Shift shift
    ) {
        var result = await http.CreateCourseOfferingHttp(campusId, courseId, courseCurriculumId, period, shift);
        return await result.DeserializeTo<CourseOfferingOut>();
    }

    public static async Task<List<CourseOfferingOut>> GetCourseOfferings(this HttpClient http)
    {
        var client = new GetCourseOfferingsClient(http);
        return await client.Get();
    }

    public static async Task<AcademicInsightsOut> GetAcademicInsights(this HttpClient http)
    {
        var client = new GetAcademicInsightsClient(http);
        return await client.Get();
    }

    public static async Task<TeacherOut> CreateTeacher(
        this HttpClient http,
        string name = "Chico",
        string email = ""
    ) {
        email = email.HasValue() ? email : TestData.Email;
        var client = new CreateTeacherClient(http);
        var response = await client.Create(name, email);
        var professor= await response.DeserializeTo<TeacherOut>();
        professor.Email = email;
        return professor;
    }

    public static async Task<List<TeacherOut>> GetTeachers(this HttpClient http)
    {
        var client = new GetTeachersClient(http);
        return await client.Get();
    }

    public static async Task<HttpResponseMessage> CreateClassHttp(
        this HttpClient http,
        Guid disciplineId,
        Guid teacherId,
        string period,
        List<ScheduleIn> schedules
    ) {
        var client = new CreateClassClient(http);
        return await client.Create(disciplineId, teacherId, period, schedules);
    }

    public static async Task<ClassOut> CreateClass(
        this HttpClient http,
        Guid disciplineId,
        Guid teacherId,
        string period,
        List<ScheduleIn> schedules
    ) {
        var result = await http.CreateClassHttp(disciplineId, teacherId, period, schedules);
        return await result.DeserializeTo<ClassOut>();
    }

    public static async Task<List<ClassOut>> GetClasses(this HttpClient http)
    {
        var client = new GetClassesClient(http);
        return await client.Get();
    }

    public static async Task<HttpResponseMessage> CreateStudentHttp(
        this HttpClient http,
        Guid courseOfferingId,
        string name = "Zezin",
        string email = ""
    ) {
        var client = new CreateStudentClient(http);
        email = email.HasValue() ? email : TestData.Email;
        return await client.Create(name, email, courseOfferingId);
    }

    public static async Task<StudentOut> CreateStudent(
        this HttpClient http,
        Guid courseOfferingId,
        string name = "Zezin",
        string email = ""
    ) {
        email = email.HasValue() ? email : TestData.Email;

        var result = await http.CreateStudentHttp(courseOfferingId, name, email);
        var student = await result.DeserializeTo<StudentOut>();
        student.Email = email;
        return student;
    }

    public static async Task<List<DisciplineOut>> GetStudentDisciplines(this HttpClient http)
    {
        var client = new GetDisciplinesClient(http);
        return await client.Get();
    }

    public static async Task<AcademicPeriodOut> CreateAcademicPeriod(this HttpClient http, string id)
    {
        var client = new CreateAcademicPeriodClient(http);
        var period = new CreateAcademicPeriodIn(id);
        var response = await client.Create(id, period.Start, period.End);
        return await response.DeserializeTo<AcademicPeriodOut>();
    }

    public static async Task<List<AcademicPeriodOut>> GetAcademicPeriods(this HttpClient http)
    {
        var client = new GetAcademicPeriodsClient(http);
        return await client.Get();
    }

    public static async Task<EnrollmentPeriodOut> CreateEnrollmentPeriod(this HttpClient http, string id, string start, string end)
    {
        var client = new CreateEnrollmentPeriodClient(http);
        var period = new CreateEnrollmentPeriodIn(id, start, end);
        var response = await client.Create(id, period.Start, period.End);
        return await response.DeserializeTo<EnrollmentPeriodOut>();
    }
    public static async Task<EnrollmentPeriodOut> CreateEnrollmentPeriod(this HttpClient http, string id, DateOnly start, DateOnly end)
    {
        var client = new CreateEnrollmentPeriodClient(http);
        var response = await client.Create(id, start, end);
        return await response.DeserializeTo<EnrollmentPeriodOut>();
    }

    public static async Task<List<EnrollmentPeriodOut>> GetEnrollmentPeriods(this HttpClient http)
    {
        var client = new GetEnrollmentPeriodsClient(http);
        return await client.Get();
    }



    // -------------------------------------------------------------------------------------------- //

    public static async Task<T> PostAsync<T>(this HttpClient client, string path, object obj)
    {
        var response = await client.PostHttpAsync(path, obj);
        return await response.DeserializeTo<T>();
    }

    public static async Task<HttpResponseMessage> PostHttpAsync(this HttpClient client, string path, object obj)
    {
        return await client.PostAsync(path, obj.ToStringContent());
    }

    public static async Task<T> GetAsync<T>(this HttpClient client, string path)
    {
        var response = await client.GetAsync(path);
        return await response.DeserializeTo<T>();
    }
}
