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
using Syki.Front.Features.Academic.CreateLessons;
using Syki.Front.Features.Academic.CreateTeacher;
using Syki.Front.Features.Academic.GetDisciplines;
using Syki.Front.Features.Academic.CreateDiscipline;
using Syki.Front.Features.Academic.GetNotifications;
using Syki.Front.Features.Academic.GetAcademicClass;
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

    public async Task<OneOf<CampusOut, ErrorOut>> UpdateCampus(Guid id, string name = "Agreste I", string city = "Caruaru - PE")
    {
        var client = new UpdateCampusClient(Cross);
        return await client.Update(id, name, city);
    }

    public async Task<OneOf<CourseOut, ErrorOut>> CreateCourse(string name = "ADS", CourseType type = CourseType.Bacharelado)
    {
        var client = new CreateCourseClient(Cross);
        return await client.Create(name, type);
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

    public async Task<OneOf<CourseCurriculumOut, ErrorOut>> CreateCourseCurriculum(
        string name,
        Guid courseId,
        List<CreateCourseCurriculumDisciplineIn> disciplines = null
    ) {
        var client = new CreateCourseCurriculumClient(Cross);
        return await client.Create(name, courseId, disciplines ?? []);
    }

    public async Task<List<CourseCurriculumOut>> GetCourseCurriculums()
    {
        var client = new GetCourseCurriculumsClient(Cross);
        return await client.Get();
    }

    public async Task<OneOf<CourseOfferingOut, ErrorOut>> CreateCourseOffering(
        Guid campusId,
        Guid courseId,
        Guid courseCurriculumId,
        string? period,
        Shift shift
    ) {
        var client = new CreateCourseOfferingClient(Cross);
        return await client.Create(campusId, courseId, courseCurriculumId, period, shift);
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

    public async Task<OneOf<TeacherOut, ErrorOut>> CreateTeacher(
        string name = "Chico",
        string email = null
    ) {
        email ??= TestData.Email;
        var client = new CreateTeacherClient(Cross);

        var response = await client.Create(name, email);

        if (response.IsSuccess())
        {
            var teacher = response.GetSuccess();
            teacher.Email = email;
            return teacher;
        }

        return response.GetError();
    }

    public async Task<List<TeacherOut>> GetTeachers()
    {
        var client = new GetTeachersClient(Cross);
        return await client.Get();
    }

    public async Task<OneOf<ClassOut, ErrorOut>> CreateClass(
        Guid disciplineId,
        Guid teacherId,
        string period,
        int vacancies,
        List<ScheduleIn> schedules
    ) {
        var client = new CreateClassClient(Cross);
        return await client.Create(disciplineId, teacherId, period, vacancies, schedules);
    }

    public async Task<List<ClassOut>> GetClasses()
    {
        var client = new GetClassesClient(Cross);
        return await client.Get();
    }

    public async Task<OneOf<GetAcademicClassOut, ErrorOut>> GetAcademicClass(Guid id)
    {
        var client = new GetAcademicClassClient(Cross);
        return await client.Get(id);
    }

    public async Task<OneOf<StudentOut, ErrorOut>> CreateStudent(
        Guid courseOfferingId,
        string name = "Zezin",
        string email = null,
        string? phoneNumber = null
    ) {
        email ??= TestData.Email;

        var client = new CreateStudentClient(Cross);
        var response = await client.Create(name, email, courseOfferingId, phoneNumber);

        if (response.IsSuccess())
        {
            var student = response.GetSuccess();
            student.Email = email;
            return student;
        }

        return response.GetError();
    }

    public async Task<List<StudentOut>> GetStudents()
    {
        var client = new GetStudentsClient(Cross);
        return await client.Get();
    }

    public async Task<OneOf<AcademicPeriodOut, ErrorOut>> CreateAcademicPeriod(
        string id,
        DateOnly? startAt = null,
        DateOnly? endAt = null
    ) {
        var client = new CreateAcademicPeriodClient(Cross);

        if (startAt == null || endAt == null)
        {
            var periodIn = new CreateAcademicPeriodIn(id);
            startAt = periodIn.StartAt;
            endAt = periodIn.EndAt;
        }

        return await client.Create(id, startAt.Value, endAt.Value);
    }

    public async Task<List<AcademicPeriodOut>> GetAcademicPeriods()
    {
        var client = new GetAcademicPeriodsClient(Cross);
        return await client.Get();
    }

    public async Task<OneOf<EnrollmentPeriodOut, ErrorOut>> CreateEnrollmentPeriod(string id, int start = -2, int end = 2)
    {
        var client = new CreateEnrollmentPeriodClient(Cross);
        var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(start));
        var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(end));
        return await client.Create(id, startDate, endDate);
    }

    public async Task<OneOf<EnrollmentPeriodOut, ErrorOut>> UpdateEnrollmentPeriod(string id, int start = -4, int end = 4)
    {
        var client = new UpdateEnrollmentPeriodClient(Cross);
        var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(start));
        var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(end));
        return await client.Update(id, startDate, endDate);
    }
    
    public async Task<List<EnrollmentPeriodOut>> GetEnrollmentPeriods()
    {
        var client = new GetEnrollmentPeriodsClient(Cross);
        return await client.Get();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> StartClass(Guid id)
    {
        var client = new StartClassClient(Cross);
        return await client.Start(id);
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> CreateClassLessons(Guid classId
    ) {
        var client = new CreateLessonsClient(Cross);
        return await client.Create(classId);
    }

    public async Task<BasicInstitutionTestDto> CreateBasicInstitutionData()
    {
        var data = new BasicInstitutionTestDto();

        var digit = DateTime.Now.Month < 6 ? 1 : 2;
        data.AcademicPeriod = await CreateAcademicPeriod($"{DateTime.Now.Year}.{digit}");
        
        data.Campus = await CreateCampus();
        data.Course = await CreateCourse();
        
        data.Disciplines.DiscreteMath = await CreateDiscipline("Matemática Discreta", [data.Course.Id]);
        data.Disciplines.IntroToWebDev = await CreateDiscipline("Introdução ao Desenvolvimento Web", [data.Course.Id]);
        data.Disciplines.HumanMachineInteractionDesign = await CreateDiscipline("Design de Interação Humano-Máquina", [data.Course.Id]);
        data.Disciplines.IntroToComputerNetworks = await CreateDiscipline("Introdução à Redes de Computadores", [data.Course.Id]);
        data.Disciplines.CumputationalThinkingAndAlgorithms = await CreateDiscipline("Pensamento Computacional e Algoritmos", [data.Course.Id]);
        data.Disciplines.IntegratorProjectOne = await CreateDiscipline("Projeto Integrador I: Concepção e Prototipação", [data.Course.Id]);

        data.CourseCurriculum = await CreateCourseCurriculum("Grade ADS 1.0", data.Course.Id,
        [
            new(data.Disciplines.DiscreteMath.Id, 1, 7, 60),
            new(data.Disciplines.IntroToWebDev.Id, 1, 6, 55),
            new(data.Disciplines.HumanMachineInteractionDesign.Id, 1, 8, 60),
            new(data.Disciplines.IntroToComputerNetworks.Id, 1, 4, 50),
            new(data.Disciplines.CumputationalThinkingAndAlgorithms.Id, 1, 6, 45),
            new(data.Disciplines.IntegratorProjectOne.Id, 1, 7, 65),
        ]);
        
        data.CourseOffering = await CreateCourseOffering(data.Campus.Id, data.Course.Id, data.CourseCurriculum.Id, data.AcademicPeriod.Id, Shift.Noturno);

        return data;
    }
}
