using Syki.Front.Features.Academic.GetCampi;
using Syki.Front.Features.Academic.GetCourses;
using Syki.Front.Features.Academic.StartClasses;
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
using Syki.Front.Features.Academic.GetAcademicClasses;
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
using Syki.Front.Features.Academic.UpdateEnrollmentPeriod;
using Syki.Front.Features.Academic.GetCoursesWithCurriculums;
using Syki.Front.Features.Academic.GetCoursesWithDisciplines;
using Syki.Front.Features.Academic.ReleaseClassesForEnrollment;

namespace Syki.Tests.Clients;

public class AcademicHttpClient(HttpClient http)
{
    public readonly HttpClient Http = http;

    public async Task<NotificationOut> CreateNotification(string title, string description, UsersGroup targetUsers, bool timeless)
    {
        var client = new CreateNotificationClient(Http);
        var response = await client.Create(title, description, targetUsers, timeless);
        return await response.DeserializeTo<NotificationOut>();
    }

    public async Task<List<NotificationOut>> GetNotifications()
    {
        var client = new GetNotificationsClient(Http);
        return await client.Get();
    }

    public async Task<CampusOut> CreateCampus(string name = "Agreste I", string city = "Caruaru - PE")
    {
        var client = new CreateCampusClient(Http);
        var response = await client.Create(name, city);
        return await response.DeserializeTo<CampusOut>();
    }

    public async Task<List<CampusOut>> GetCampi()
    {
        var client = new GetCampiClient(Http);
        return await client.Get();
    }

    public async Task<DisciplineOut> CreateDiscipline(string name = "Banco de Dados", List<Guid> courses = null)
    {
        var client = new CreateDisciplineClient(Http);
        var response = await client.Create(name, courses ?? []);
        return await response.DeserializeTo<DisciplineOut>();
    }

    public async Task<List<DisciplineOut>> GetDisciplines()
    {
        var client = new GetDisciplinesClient(Http);
        return await client.Get();
    }

    public async Task<List<CourseDisciplineOut>> GetCourseDisciplines(Guid courseId)
    {
        var client = new GetCourseDisciplinesClient(Http);
        return await client.Get(courseId);
    }

    public async Task<OneOf<CampusOut, ErrorOut>> UpdateCampus(Guid id, string name = "Agreste I", string city = "Caruaru - PE")
    {
        var client = new UpdateCampusClient(Http);
        return await client.Update(id, name, city);
    }

    public async Task<OneOf<CourseOut, ErrorOut>> CreateCourse(string name = "ADS", CourseType type = CourseType.Bacharelado)
    {
        var client = new CreateCourseClient(Http);
        return await client.Create(name, type);
    }

    public async Task<List<CourseOut>> GetCourses()
    {
        var client = new GetCoursesClient(Http);
        return await client.Get();
    }

    public async Task<List<CourseOut>> GetCoursesWithCurriculums()
    {
        var client = new GetCoursesWithCurriculumsClient(Http);
        return await client.Get();
    }

    public async Task<List<CourseOut>> GetCoursesWithDisciplines()
    {
        var client = new GetCoursesWithDisciplinesClient(Http);
        return await client.Get();
    }

    public async Task<OneOf<CourseCurriculumOut, ErrorOut>> CreateCourseCurriculum(
        string name,
        Guid courseId,
        List<CreateCourseCurriculumDisciplineIn> disciplines = null
    ) {
        var client = new CreateCourseCurriculumClient(Http);
        return await client.Create(name, courseId, disciplines ?? []);
    }

    public async Task<List<CourseCurriculumOut>> GetCourseCurriculums()
    {
        var client = new GetCourseCurriculumsClient(Http);
        return await client.Get();
    }

    public async Task<OneOf<CourseOfferingOut, ErrorOut>> CreateCourseOffering(
        Guid campusId,
        Guid courseId,
        Guid courseCurriculumId,
        string? period,
        Shift shift
    ) {
        var client = new CreateCourseOfferingClient(Http);
        return await client.Create(campusId, courseId, courseCurriculumId, period, shift);
    }

    public async Task<List<CourseOfferingOut>> GetCourseOfferings()
    {
        var client = new GetCourseOfferingsClient(Http);
        return await client.Get();
    }

    public async Task<AcademicInsightsOut> GetAcademicInsights()
    {
        var client = new GetAcademicInsightsClient(Http);
        return await client.Get();
    }

    public async Task<OneOf<TeacherOut, ErrorOut>> CreateTeacher(
        string name = "Chico",
        string email = null
    ) {
        email ??= TestData.Email;
        var client = new CreateTeacherClient(Http);

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
        var client = new GetTeachersClient(Http);
        return await client.Get();
    }

    public async Task<OneOf<ClassOut, ErrorOut>> CreateClass(
        Guid disciplineId,
        Guid teacherId,
        string period,
        int vacancies,
        List<ScheduleIn> schedules
    ) {
        var client = new CreateClassClient(Http);
        return await client.Create(disciplineId, teacherId, period, vacancies, schedules);
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> ReleaseClassesForEnrollment(List<Guid> classes)
    {
        var client = new ReleaseClassesForEnrollmentClient(Http);
        return await client.Release(classes);
    }

    public async Task<List<ClassOut>> GetAcademicClasses(GetAcademicClassesIn query = null)
    {
        var client = new GetAcademicClassesClient(Http);
        return await client.Get(query);
    }

    public async Task<OneOf<GetAcademicClassOut, ErrorOut>> GetAcademicClass(Guid id)
    {
        var client = new GetAcademicClassClient(Http);
        return await client.Get(id);
    }

    public async Task<OneOf<StudentOut, ErrorOut>> CreateStudent(
        Guid courseOfferingId,
        string name = "Zezin",
        string email = null,
        string? phoneNumber = null
    ) {
        email ??= TestData.Email;

        var client = new CreateStudentClient(Http);
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
        var client = new GetStudentsClient(Http);
        return await client.Get();
    }

    public async Task<OneOf<AcademicPeriodOut, ErrorOut>> CreateAcademicPeriod(
        string id,
        DateOnly? startAt = null,
        DateOnly? endAt = null
    ) {
        var client = new CreateAcademicPeriodClient(Http);

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
        var client = new GetAcademicPeriodsClient(Http);
        return await client.Get();
    }

    public async Task<OneOf<EnrollmentPeriodOut, ErrorOut>> CreateEnrollmentPeriod(string id, int start = -2, int end = 2)
    {
        var client = new CreateEnrollmentPeriodClient(Http);
        var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(start));
        var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(end));
        return await client.Create(id, startDate, endDate);
    }

    public async Task<OneOf<EnrollmentPeriodOut, ErrorOut>> UpdateEnrollmentPeriod(string id, int start = -4, int end = 4)
    {
        var client = new UpdateEnrollmentPeriodClient(Http);
        var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(start));
        var endDate = DateOnly.FromDateTime(DateTime.Now.AddDays(end));
        return await client.Update(id, startDate, endDate);
    }
    
    public async Task<List<EnrollmentPeriodOut>> GetEnrollmentPeriods()
    {
        var client = new GetEnrollmentPeriodsClient(Http);
        return await client.Get();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> StartClasses(List<Guid> classes)
    {
        var client = new StartClassesClient(Http);
        return await client.Start(classes);
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> CreateClassLessons(Guid classId)
    {
        var client = new CreateLessonsClient(Http);
        return await client.Create(classId);
    }

    public async Task<BasicInstitutionTestDto> CreateBasicInstitutionData()
    {
        var data = new BasicInstitutionTestDto();

        data.AcademicPeriod1 = await CreateAcademicPeriod($"{DateTime.Now.Year}.1");
        data.AcademicPeriod2 = await CreateAcademicPeriod($"{DateTime.Now.Year}.2");

        data.Campus = await CreateCampus();

        // Ads
        data.AdsCourse = await CreateCourse("Análise e Desenvolvimento de Sistemas", CourseType.Tecnologo);

        data.AdsDisciplines.DiscreteMath = await CreateDiscipline("Matemática Discreta", [data.AdsCourse.Id]);
        data.AdsDisciplines.IntroToWebDev = await CreateDiscipline("Introdução ao Desenvolvimento Web", [data.AdsCourse.Id]);
        data.AdsDisciplines.HumanMachineInteractionDesign = await CreateDiscipline("Design de Interação Humano-Máquina", [data.AdsCourse.Id]);
        data.AdsDisciplines.IntroToComputerNetworks = await CreateDiscipline("Introdução à Redes de Computadores", [data.AdsCourse.Id]);
        data.AdsDisciplines.ComputationalThinkingAndAlgorithms = await CreateDiscipline("Pensamento Computacional e Algoritmos", [data.AdsCourse.Id]);
        data.AdsDisciplines.IntegratorProjectOne = await CreateDiscipline("Projeto Integrador I: Concepção e Prototipação", [data.AdsCourse.Id]);

        data.AdsDisciplines.Arch = await CreateDiscipline("Arquitetura de Computadores e Sistemas Operacionais", [data.AdsCourse.Id]);
        data.AdsDisciplines.Databases = await CreateDiscipline("Banco de Dados", [data.AdsCourse.Id]);
        data.AdsDisciplines.DataStructures = await CreateDiscipline("Estrutura de Dados", [data.AdsCourse.Id]);
        data.AdsDisciplines.InfoAndSociety = await CreateDiscipline("Informática e Sociedade", [data.AdsCourse.Id]);
        data.AdsDisciplines.Poo = await CreateDiscipline("Programação Orientada a Objetos", [data.AdsCourse.Id]);
        data.AdsDisciplines.IntegratorProjectTwo = await CreateDiscipline("Projeto Integrador II: Modelagem de Banco de Dados", [data.AdsCourse.Id]);

        data.AdsCourseCurriculum = await CreateCourseCurriculum("Grade ADS 1.0", data.AdsCourse.Id,
        [
            new(data.AdsDisciplines.DiscreteMath.Id, 1, 7, 60),
            new(data.AdsDisciplines.IntroToWebDev.Id, 1, 6, 55),
            new(data.AdsDisciplines.HumanMachineInteractionDesign.Id, 1, 8, 60),
            new(data.AdsDisciplines.IntroToComputerNetworks.Id, 1, 4, 50),
            new(data.AdsDisciplines.ComputationalThinkingAndAlgorithms.Id, 1, 6, 45),
            new(data.AdsDisciplines.IntegratorProjectOne.Id, 1, 7, 65),

            new(data.AdsDisciplines.Arch.Id, 1, 7, 60),
            new(data.AdsDisciplines.Databases.Id, 1, 6, 55),
            new(data.AdsDisciplines.DataStructures.Id, 1, 8, 60),
            new(data.AdsDisciplines.InfoAndSociety.Id, 1, 4, 50),
            new(data.AdsDisciplines.Poo.Id, 1, 6, 45),
            new(data.AdsDisciplines.IntegratorProjectTwo.Id, 1, 7, 65),
        ]);
        
        data.AdsCourseOffering = await CreateCourseOffering(data.Campus.Id, data.AdsCourse.Id, data.AdsCourseCurriculum.Id, data.AcademicPeriod2.Id, Shift.Noturno);

        // Direito
        data.DireitoCourse = await CreateCourse("Direito", CourseType.Bacharelado);

        data.DireitoDisciplines.PhilosophicalBases = await CreateDiscipline("Bases Filosóficas", [data.DireitoCourse.Id]);
        data.DireitoDisciplines.CommunicationAndLegalArgumentation = await CreateDiscipline("Comunicação e Argumentação Jurídica", [data.DireitoCourse.Id]);
        data.DireitoDisciplines.ManSocietyAndLaw = await CreateDiscipline("Homem, Sociedade e Direito", [data.DireitoCourse.Id]);
        data.DireitoDisciplines.PoliticsAndStateInFocus = await CreateDiscipline("Política e Estado em Foco", [data.DireitoCourse.Id]);
        data.DireitoDisciplines.GeneralTheoryOfLaw = await CreateDiscipline("Teoria Geral do Direito", [data.DireitoCourse.Id]);

        data.DireitoCourseCurriculum = await CreateCourseCurriculum("Grade Direito 1.0", data.DireitoCourse.Id,
        [
            new(data.DireitoDisciplines.PhilosophicalBases.Id, 1, 7, 60),
            new(data.DireitoDisciplines.CommunicationAndLegalArgumentation.Id, 1, 6, 55),
            new(data.DireitoDisciplines.ManSocietyAndLaw.Id, 1, 8, 60),
            new(data.DireitoDisciplines.PoliticsAndStateInFocus.Id, 1, 4, 50),
            new(data.DireitoDisciplines.GeneralTheoryOfLaw.Id, 1, 6, 45),
        ]);
        
        data.DireitoCourseOffering = await CreateCourseOffering(data.Campus.Id, data.DireitoCourse.Id, data.DireitoCourseCurriculum.Id, data.AcademicPeriod2.Id, Shift.Noturno);

        return data;
    }
}
