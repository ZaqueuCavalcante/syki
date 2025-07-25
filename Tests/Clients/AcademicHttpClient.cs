using Syki.Front.Features.Academic.GetCampi;
using Syki.Front.Features.Academic.GetCourses;
using Syki.Front.Features.Academic.CreateClass;
using Syki.Front.Features.Academic.GetTeachers;
using Syki.Front.Features.Academic.GetStudents;
using Syki.Front.Features.Academic.StartClasses;
using Syki.Front.Features.Academic.CreateCampus;
using Syki.Front.Features.Academic.UpdateCampus;
using Syki.Front.Features.Academic.CreateCourse;
using Syki.Front.Features.Academic.CreateStudent;
using Syki.Front.Features.Academic.CreateTeacher;
using Syki.Front.Features.Academic.GetDisciplines;
using Syki.Front.Features.Academic.FinalizeClasses;
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
using Syki.Front.Features.Academic.CreateWebhookSubscription;
using Syki.Front.Features.Academic.AddDisciplinePreRequisites;
using Syki.Front.Features.Academic.ReleaseClassesForEnrollment;

namespace Syki.Tests.Clients;

public class AcademicHttpClient(HttpClient http)
{
    public readonly HttpClient Http = http;

    public async Task<NotificationOut> CreateNotification(
        string title,
        string description,
        UsersGroup targetUsers,
        bool timeless
    ) {
        var client = new CreateNotificationClient(Http);
        var response = await client.Create(title, description, targetUsers, timeless);
        return await response.DeserializeTo<NotificationOut>();
    }

    public async Task<List<NotificationOut>> GetNotifications()
    {
        var client = new GetNotificationsClient(Http);
        return await client.Get();
    }

    public async Task<CampusOut> CreateCampus(
        string name = "Agreste I",
        BrazilState state = BrazilState.PE,
        string city = "Caruaru",
        int capacity = 100
    ) {
        var client = new CreateCampusClient(Http);
        var response = await client.Create(name, state, city, capacity);
        return await response.DeserializeTo<CampusOut>();
    }

    public async Task<List<CampusOut>> GetCampi()
    {
        var client = new GetCampiClient(Http);
        return await client.Get();
    }

    public async Task<DisciplineOut> CreateDiscipline(
        string name = "Banco de Dados",
        List<Guid> courses = null
    ) {
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

    public async Task<OneOf<CampusOut, ErrorOut>> UpdateCampus(
        Guid id,
        string name = "Agreste I",
        BrazilState state = BrazilState.PE,
        string city = "Caruaru",
        int capacity = 100
    ) {
        var client = new UpdateCampusClient(Http);
        return await client.Update(id, name, state, city, capacity);
    }

    public async Task<OneOf<CourseOut, ErrorOut>> CreateCourse(
        string name, 
        CourseType type, 
        List<string> disciplines
    ) {
        var client = new CreateCourseClient(Http);
        return await client.Create(name, type, disciplines);
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
        var startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(start));
        var endDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(end));
        return await client.Create(id, startDate, endDate);
    }

    public async Task<OneOf<EnrollmentPeriodOut, ErrorOut>> UpdateEnrollmentPeriod(string id, int start = -4, int end = 4)
    {
        var client = new UpdateEnrollmentPeriodClient(Http);
        var startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(start));
        var endDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(end));
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

    public async Task<OneOf<SuccessOut, ErrorOut>> FinalizeClasses(List<Guid> classes)
    {
        var client = new FinalizeClassesClient(Http);
        return await client.Finalize(classes);
    }

    public async Task<OneOf<CreateWebhookSubscriptionOut, ErrorOut>> CreateWebhookSubscription(
        string name = "Aluno Criado",
        string url = "https://example.com/webhook",
        List<WebhookEventType> events = null,
        WebhookAuthenticationType authenticationType = WebhookAuthenticationType.ApiKey,
        string? apiKey = "z3Q6uDUJYTDCIo16myBKZrlCS63IvpCUOAE5X"
    ) {
        events ??= [WebhookEventType.StudentCreated];
        var client = new CreateWebhookSubscriptionClient(Http);
        return await client.Create(name, url, events, authenticationType, apiKey);
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> AddDisciplinePreRequisites(
        Guid courseCurriculumId,
        Guid disciplineId,
        List<Guid> preRequisites
    ) {
        var client = new AddDisciplinePreRequisitesClient(Http);

        return await client.Add(courseCurriculumId, disciplineId, preRequisites);
    }

    public async Task<BasicInstitutionTestDto> CreateBasicInstitutionData()
    {
        var data = new BasicInstitutionTestDto();

        data.AcademicPeriod1 = await CreateAcademicPeriod($"{DateTime.UtcNow.Year}.1");
        data.AcademicPeriod2 = await CreateAcademicPeriod($"{DateTime.UtcNow.Year}.2");

        data.Campus = await CreateCampus();

        // Ads
        var adsDisciplines = new List<string>()
        {
            "Matemática Discreta",
            "Introdução ao Desenvolvimento Web",
            "Design de Interação Humano-Máquina",
            "Introdução à Redes de Computadores",
            "Pensamento Computacional e Algoritmos",
            "Projeto Integrador I: Concepção e Prototipação",

            "Arquitetura de Computadores e Sistemas Operacionais",
            "Banco de Dados",
            "Estrutura de Dados",
            "Informática e Sociedade",
            "Programação Orientada a Objetos",
            "Projeto Integrador II: Modelagem de Banco de Dados",
        };

        data.AdsCourse = await CreateCourse("Análise e Desenvolvimento de Sistemas", CourseType.Tecnologo, adsDisciplines);

        data.AdsDisciplines.DiscreteMath = data.AdsCourse.Disciplines[0];
        data.AdsDisciplines.IntroToWebDev = data.AdsCourse.Disciplines[1];
        data.AdsDisciplines.HumanMachineInteractionDesign = data.AdsCourse.Disciplines[2];
        data.AdsDisciplines.IntroToComputerNetworks = data.AdsCourse.Disciplines[3];
        data.AdsDisciplines.ComputationalThinkingAndAlgorithms = data.AdsCourse.Disciplines[4];
        data.AdsDisciplines.IntegratorProjectOne = data.AdsCourse.Disciplines[5];

        data.AdsDisciplines.Arch = data.AdsCourse.Disciplines[6];
        data.AdsDisciplines.Databases = data.AdsCourse.Disciplines[7];
        data.AdsDisciplines.DataStructures = data.AdsCourse.Disciplines[8];
        data.AdsDisciplines.InfoAndSociety = data.AdsCourse.Disciplines[9];
        data.AdsDisciplines.Poo = data.AdsCourse.Disciplines[10];
        data.AdsDisciplines.IntegratorProjectTwo = data.AdsCourse.Disciplines[11];

        data.AdsCourseCurriculum = await CreateCourseCurriculum("Grade ADS 1.0", data.AdsCourse.Id,
        [
            new(data.AdsDisciplines.DiscreteMath.Id, 1, 7, 60),
            new(data.AdsDisciplines.IntroToWebDev.Id, 1, 6, 55),
            new(data.AdsDisciplines.HumanMachineInteractionDesign.Id, 1, 8, 60),
            new(data.AdsDisciplines.IntroToComputerNetworks.Id, 1, 4, 50),
            new(data.AdsDisciplines.ComputationalThinkingAndAlgorithms.Id, 1, 6, 45),
            new(data.AdsDisciplines.IntegratorProjectOne.Id, 1, 7, 65),

            new(data.AdsDisciplines.Arch.Id, 2, 7, 60),
            new(data.AdsDisciplines.Databases.Id, 2, 6, 55),
            new(data.AdsDisciplines.DataStructures.Id, 2, 8, 60),
            new(data.AdsDisciplines.InfoAndSociety.Id, 2, 4, 50),
            new(data.AdsDisciplines.Poo.Id, 2, 6, 45),
            new(data.AdsDisciplines.IntegratorProjectTwo.Id, 2, 7, 65),
        ]);

        data.AdsCourseOffering = await CreateCourseOffering(data.Campus.Id, data.AdsCourse.Id, data.AdsCourseCurriculum.Id, data.AcademicPeriod2.Id, Shift.Noturno);

        // Direito
        var direitoDisciplines = new List<string>()
        {
            "Bases Filosóficas",
            "Comunicação e Argumentação Jurídica",
            "Homem, Sociedade e Direito",
            "Política e Estado em Foco",
            "Teoria Geral do Direito",
        };

        data.DireitoCourse = await CreateCourse("Direito", CourseType.Bacharelado, direitoDisciplines);

        data.DireitoDisciplines.PhilosophicalBases = data.DireitoCourse.Disciplines[0];
        data.DireitoDisciplines.CommunicationAndLegalArgumentation = data.DireitoCourse.Disciplines[1];
        data.DireitoDisciplines.ManSocietyAndLaw = data.DireitoCourse.Disciplines[2];
        data.DireitoDisciplines.PoliticsAndStateInFocus = data.DireitoCourse.Disciplines[3];
        data.DireitoDisciplines.GeneralTheoryOfLaw = data.DireitoCourse.Disciplines[4];

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

    public async Task AddStartedAdsClasses(BasicInstitutionTestDto data, BackFactory back)
    {
        var period = data.AcademicPeriod1.Id;

        await CreateEnrollmentPeriod(period, -2, 2);

        data.Teacher = await CreateTeacher();
        data.AdsClasses.HumanMachineInteractionDesign = await CreateClass(data.AdsDisciplines.HumanMachineInteractionDesign.Id, data.Teacher.Id, period, 45, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);
        data.AdsClasses.IntroToComputerNetworks = await CreateClass(data.AdsDisciplines.IntroToComputerNetworks.Id, data.Teacher.Id, period, 45, [new(Day.Tuesday, Hour.H07_00, Hour.H10_00)]);
        data.AdsClasses.IntroToWebDev = await CreateClass(data.AdsDisciplines.IntroToWebDev.Id, data.Teacher.Id, period, 45, [new(Day.Wednesday, Hour.H07_00, Hour.H10_00)]);
        data.AdsClasses.DiscreteMath = await CreateClass(data.AdsDisciplines.DiscreteMath.Id, data.Teacher.Id, period, 40, [new(Day.Thursday, Hour.H07_00, Hour.H10_00)]);
        data.AdsClasses.ComputationalThinkingAndAlgorithms = await CreateClass(data.AdsDisciplines.ComputationalThinkingAndAlgorithms.Id, data.Teacher.Id, period, 45, [new(Day.Friday, Hour.H07_00, Hour.H10_00)]);
        data.AdsClasses.IntegratorProjectOne = await CreateClass(data.AdsDisciplines.IntegratorProjectOne.Id, data.Teacher.Id, period, 45, [new(Day.Saturday, Hour.H07_00, Hour.H10_00)]);

        await ReleaseClassesForEnrollment([
            data.AdsClasses.DiscreteMath.Id,
            data.AdsClasses.IntroToWebDev.Id,
            data.AdsClasses.HumanMachineInteractionDesign.Id,
            data.AdsClasses.IntroToComputerNetworks.Id,
            data.AdsClasses.ComputationalThinkingAndAlgorithms.Id,
            data.AdsClasses.IntegratorProjectOne.Id
        ]);

        data.Student = await CreateStudent(data.AdsCourseOffering.Id);
        var studentClient = await back.LoggedAsStudent(data.Student.Email);
        await studentClient.CreateStudentEnrollment([
            data.AdsClasses.DiscreteMath.Id,
            data.AdsClasses.IntroToWebDev.Id,
            data.AdsClasses.HumanMachineInteractionDesign.Id,
            data.AdsClasses.IntroToComputerNetworks.Id,
            data.AdsClasses.ComputationalThinkingAndAlgorithms.Id,
            data.AdsClasses.IntegratorProjectOne.Id
        ]);

        await UpdateEnrollmentPeriod(period, -2, -1);
        await StartClasses([
            data.AdsClasses.DiscreteMath.Id,
            data.AdsClasses.IntroToWebDev.Id,
            data.AdsClasses.HumanMachineInteractionDesign.Id,
            data.AdsClasses.IntroToComputerNetworks.Id,
            data.AdsClasses.ComputationalThinkingAndAlgorithms.Id,
            data.AdsClasses.IntegratorProjectOne.Id
        ]);
    }
}
