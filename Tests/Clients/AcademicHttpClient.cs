using Syki.Front.Features.Academic.GetCampi;
using Syki.Front.Features.Academic.GetCourses;
using Syki.Front.Features.Academic.GetClasses;
using Syki.Front.Features.Academic.StartClass;
using Syki.Front.Features.Academic.CreateClass;
using Syki.Front.Features.Academic.GetTeachers;
using Syki.Front.Features.Academic.GetStudents;
using Syki.Front.Features.Academic.CreateCampus;
using Syki.Front.Features.Academic.UpdateCampus;
using Syki.Front.Features.Academic.CreateCourse;
using Syki.Front.Features.Academic.CreateStudent;
using Syki.Front.Features.Academic.CreateTeacher;
using Syki.Front.Features.Academic.GetDisciplines;
using Syki.Front.Features.Academic.CreateDiscipline;
using Syki.Front.Features.Academic.GetNotifications;
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
using Syki.Front.Features.Academic.GetCoursesWithCurriculums;
using Syki.Front.Features.Academic.GetCoursesWithDisciplines;

namespace Syki.Tests.Clients;

public class AcademicHttpClient(HttpClient http)
{
    public readonly HttpClient Cross = http;

    public async Task<NotificationOut> CreateNotification(string title, string description, UsersGroup targetUsers, bool timeless)
    {
        var client = new CreateNotificationClient(Cross);
        var response = await client.Create(title, description, targetUsers, timeless);
        return await response.DeserializeTo<NotificationOut>();
    }

    public async Task<List<NotificationOut>> GetNotifications()
    {
        var client = new GetNotificationsClient(Cross);
        return await client.Get();
    }

    public async Task<CampusOut> CreateCampus(string name = "Agreste I", string city = "Caruaru - PE")
    {
        var client = new CreateCampusClient(Cross);
        var response = await client.Create(name, city);
        return await response.DeserializeTo<CampusOut>();
    }

    public async Task<List<CampusOut>> GetCampi()
    {
        var client = new GetCampiClient(Cross);
        return await client.Get();
    }

    public async Task<DisciplineOut> CreateDiscipline(string name = "Banco de Dados", List<Guid> courses = null)
    {
        var client = new CreateDisciplineClient(Cross);
        var response = await client.Create(name, courses ?? []);
        return await response.DeserializeTo<DisciplineOut>();
    }

    public async Task<List<DisciplineOut>> GetDisciplines()
    {
        var client = new GetDisciplinesClient(Cross);
        return await client.Get();
    }

    public async Task<List<CourseDisciplineOut>> GetCourseDisciplines(Guid courseId)
    {
        var client = new GetCourseDisciplinesClient(Cross);
        return await client.Get(courseId);
    }

    public async Task<HttpResponseMessage> UpdateCampusHttp(Guid id, string name = "Agreste I", string city = "Caruaru - PE")
    {
        var client = new UpdateCampusClient(Cross);
        return await client.Update(id, name, city);
    }

    public async Task<CampusOut> UpdateCampus(Guid id, string name = "Agreste I", string city = "Caruaru - PE")
    {
        var response = await UpdateCampusHttp(id, name, city);
        return await response.DeserializeTo<CampusOut>();
    }

    public async Task<CourseOut> CreateCourse(string name = "ADS", CourseType tipo = CourseType.Bacharelado)
    {
        var client = new CreateCourseClient(Cross);
        var response = await client.Create(name, tipo);
        return await response.DeserializeTo<CourseOut>();
    }

    public async Task<(CourseOut, HttpResponseMessage)> CreateCourseTuple(string name = "ADS", CourseType tipo = CourseType.Bacharelado)
    {
        var client = new CreateCourseClient(Cross);
        var response = await client.Create(name, tipo);
        return (await response.DeserializeTo<CourseOut>(), response);
    }

    public async Task<List<CourseOut>> GetCourses()
    {
        var client = new GetCoursesClient(Cross);
        return await client.Get();
    }

    public async Task<List<CourseOut>> GetCoursesWithCurriculums()
    {
        var client = new GetCoursesWithCurriculumsClient(Cross);
        return await client.Get();
    }

    public async Task<List<CourseOut>> GetCoursesWithDisciplines()
    {
        var client = new GetCoursesWithDisciplinesClient(Cross);
        return await client.Get();
    }

    public async Task<(CourseCurriculumOut, HttpResponseMessage)> CreateCourseCurriculumTuple(
        string name,
        Guid courseId,
        List<CreateCourseCurriculumDisciplineIn> disciplines = null
    ) {
        var client = new CreateCourseCurriculumClient(Cross);
        var response = await client.Create(name, courseId, disciplines ?? []);
        return (await response.DeserializeTo<CourseCurriculumOut>(), response);
    }

    public async Task<CourseCurriculumOut> CreateCourseCurriculum(
        string name,
        Guid courseId,
        List<CreateCourseCurriculumDisciplineIn> disciplines = null
    ) {
        var client = new CreateCourseCurriculumClient(Cross);
        var response = await client.Create(name, courseId, disciplines ?? []);
        return await response.DeserializeTo<CourseCurriculumOut>();
    }

    public async Task<List<CourseCurriculumOut>> GetCourseCurriculums()
    {
        var client = new GetCourseCurriculumsClient(Cross);
        return await client.Get();
    }

    public async Task<HttpResponseMessage> CreateCourseOfferingHttp(
        Guid campusId,
        Guid courseId,
        Guid courseCurriculumId,
        string? period,
        Shift shift
    ) {
        var client = new CreateCourseOfferingClient(Cross);

        return await client.Create(campusId, courseId, courseCurriculumId, period, shift);
    }

    public async Task<CourseOfferingOut> CreateCourseOffering(
        Guid campusId,
        Guid courseId,
        Guid courseCurriculumId,
        string? period,
        Shift shift
    ) {
        var result = await CreateCourseOfferingHttp(campusId, courseId, courseCurriculumId, period, shift);
        return await result.DeserializeTo<CourseOfferingOut>();
    }

    public async Task<List<CourseOfferingOut>> GetCourseOfferings()
    {
        var client = new GetCourseOfferingsClient(Cross);
        return await client.Get();
    }

    public async Task<AcademicInsightsOut> GetAcademicInsights()
    {
        var client = new GetAcademicInsightsClient(Cross);
        return await client.Get();
    }

    public async Task<TeacherOut> CreateTeacher(
        string name = "Chico",
        string email = ""
    ) {
        email = email.HasValue() ? email : TestData.Email;
        var client = new CreateTeacherClient(Cross);
        var response = await client.Create(name, email);
        var teacher= await response.DeserializeTo<TeacherOut>();
        teacher.Email = email;
        return teacher;
    }

    public async Task<(TeacherOut, HttpResponseMessage)> CreateTeacherTuple(
        string name = "Chico",
        string email = ""
    ) {
        var client = new CreateTeacherClient(Cross);
        var response = await client.Create(name, email);
        var teacher= await response.DeserializeTo<TeacherOut>();
        teacher.Email = email;
        return (teacher, response);
    }

    public async Task<List<TeacherOut>> GetTeachers()
    {
        var client = new GetTeachersClient(Cross);
        return await client.Get();
    }

    public async Task<HttpResponseMessage> CreateClassHttp(
        Guid disciplineId,
        Guid teacherId,
        string period,
        int vacancies,
        List<ScheduleIn> schedules
    ) {
        var client = new CreateClassClient(Cross);
        return await client.Create(disciplineId, teacherId, period, vacancies, schedules);
    }

    public async Task<ClassOut> CreateClass(
        Guid disciplineId,
        Guid teacherId,
        string period,
        int vacancies,
        List<ScheduleIn> schedules
    ) {
        var result = await CreateClassHttp(disciplineId, teacherId, period, vacancies, schedules);
        return await result.DeserializeTo<ClassOut>();
    }

    public async Task<List<ClassOut>> GetClasses()
    {
        var client = new GetClassesClient(Cross);
        return await client.Get();
    }

    public async Task<(StudentOut, HttpResponseMessage)> CreateStudentTuple(
        Guid courseOfferingId,
        string name = "Zezin",
        string email = ""
    ) {
        var client = new CreateStudentClient(Cross);
        var response = await client.Create(name, email, courseOfferingId);
        return (await response.DeserializeTo<StudentOut>(), response);
    }

    public async Task<StudentOut> CreateStudent(
        Guid courseOfferingId,
        string name = "Zezin",
        string email = ""
    ) {
        email = email.HasValue() ? email : TestData.Email;
        var (student, _) = await CreateStudentTuple(courseOfferingId, name, email);
        student.Email = email;
        return student;
    }

    public async Task<List<StudentOut>> GetStudents()
    {
        var client = new GetStudentsClient(Cross);
        return await client.Get();
    }

    public async Task<HttpResponseMessage> CreateAcademicPeriodHttp(string id)
    {
        var client = new CreateAcademicPeriodClient(Cross);
        var period = new CreateAcademicPeriodIn(id);
        return await client.Create(id, period.StartAt, period.EndAt);
    }

    public async Task<AcademicPeriodOut> CreateAcademicPeriod(string id)
    {
        var response = await CreateAcademicPeriodHttp(id);
        return await response.DeserializeTo<AcademicPeriodOut>();
    }

    public async Task<List<AcademicPeriodOut>> GetAcademicPeriods()
    {
        var client = new GetAcademicPeriodsClient(Cross);
        return await client.Get();
    }

    public async Task<HttpResponseMessage> CreateEnrollmentPeriodHttp(string id, string start, string end)
    {
        var client = new CreateEnrollmentPeriodClient(Cross);
        var period = new CreateEnrollmentPeriodIn(id, start, end);
        return await client.Create(id, period.StartAt, period.EndAt);
    }
    public async Task<EnrollmentPeriodOut> CreateEnrollmentPeriod(string id, int start = -2, int end = 2)
    {
        var client = new CreateEnrollmentPeriodClient(Cross);
        var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(start));
        var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(end));
        var response = await client.Create(id, startDate, endDate);
        return await response.DeserializeTo<EnrollmentPeriodOut>();
    }

    public async Task<List<EnrollmentPeriodOut>> GetEnrollmentPeriods()
    {
        var client = new GetEnrollmentPeriodsClient(Cross);
        return await client.Get();
    }

    public async Task<HttpResponseMessage> StartClass(Guid id)
    {
        var client = new StartClassClient(Cross);
        return await client.Start(id);
    }
}
