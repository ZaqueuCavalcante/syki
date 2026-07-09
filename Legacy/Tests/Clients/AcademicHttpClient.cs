using System.Net.Http.Json;

namespace Estud.Tests.Clients;

public class AcademicHttpClient(HttpClient http)
{
    public readonly HttpClient Http = http;

    public async Task<OneOf<CreateCampusOut, ErrorOut>> CreateCampus(
        string name = "Agreste I",
        BrazilState? state = BrazilState.PE,
        string city = "Caruaru",
        int capacity = 100
    ) {
        var data = new CreateCampusIn { Name = name, State = state, City = city, Capacity = capacity };
        var response = await Http.PostAsJsonAsync("/academic/campi", data);
        return await response.Resolve<CreateCampusOut>();
    }

    public async Task<GetCampiOut> GetCampi()
    {
        return await Http.GetFromJsonAsync<GetCampiOut>("/academic/campi", HttpConfigs.JsonOptions) ?? new();
    }

    public async Task<OneOf<UpdateCampusOut, ErrorOut>> UpdateCampus(
        Guid id,
        string name = "Agreste I",
        BrazilState? state = BrazilState.PE,
        string city = "Caruaru",
        int capacity = 100
    ) {
        var data = new UpdateCampusIn { Id = id, Name = name, State = state, City = city, Capacity = capacity };
        var response = await Http.PutAsJsonAsync("/academic/campi", data);
        return await response.Resolve<UpdateCampusOut>();
    }

    public async Task<OneOf<CreateCourseOut, ErrorOut>> CreateCourse(
        string name,
        CourseType? type,
        List<string> disciplines
    ) {
        var data = new CreateCourseIn { Name = name, Type = type, Disciplines = disciplines };
        var response = await Http.PostAsJsonAsync("/academic/courses", data);
        return await response.Resolve<CreateCourseOut>();
    }

    public async Task<GetCoursesOut> GetCourses()
    {
        return await Http.GetFromJsonAsync<GetCoursesOut>("/academic/courses", HttpConfigs.JsonOptions) ?? new();
    }

    public async Task<GetCoursesWithCurriculumsOut> GetCoursesWithCurriculums()
    {
        return await Http.GetFromJsonAsync<GetCoursesWithCurriculumsOut>("/academic/courses/with-curriculums", HttpConfigs.JsonOptions) ?? new();
    }

    public async Task<GetCoursesWithDisciplinesOut> GetCoursesWithDisciplines()
    {
        return await Http.GetFromJsonAsync<GetCoursesWithDisciplinesOut>("/academic/courses/with-disciplines", HttpConfigs.JsonOptions) ?? new();
    }

    public async Task<NotificationOut> CreateNotification(
        string title,
        string description,
        UsersGroup targetUsers,
        bool timeless
    ) {
        var data = new CreateNotificationIn(title, description, targetUsers, timeless);
        var response = await Http.PostAsJsonAsync("/academic/notifications", data);
        return await response.DeserializeTo<NotificationOut>();
    }

    public async Task<List<NotificationOut>> GetNotifications()
    {
        return await Http.GetFromJsonAsync<List<NotificationOut>>("/academic/notifications", HttpConfigs.JsonOptions) ?? [];
    }

    public async Task<DisciplineOut> CreateDiscipline(
        string name = "Banco de Dados",
        List<Guid> courses = null
    ) {
        var data = new CreateDisciplineIn { Name = name, Courses = courses ?? [] };
        var response = await Http.PostAsJsonAsync("/academic/disciplines", data);
        return await response.DeserializeTo<DisciplineOut>();
    }

    public async Task<List<DisciplineOut>> GetDisciplines()
    {
        return await Http.GetFromJsonAsync<List<DisciplineOut>>("/academic/disciplines", HttpConfigs.JsonOptions) ?? [];
    }

    public async Task<List<CourseDisciplineOut>> GetCourseDisciplines(Guid courseId)
    {
        return await Http.GetFromJsonAsync<List<CourseDisciplineOut>>($"/academic/courses/{courseId}/disciplines", HttpConfigs.JsonOptions) ?? [];
    }

    public async Task<OneOf<CourseCurriculumOut, ErrorOut>> CreateCourseCurriculum(
        string name,
        Guid courseId,
        List<CreateCourseCurriculumDisciplineIn> disciplines = null
    ) {
        var data = new CreateCourseCurriculumIn
        {
            Name = name,
            CourseId = courseId,
            Disciplines = disciplines ?? [],
        };
        var response = await Http.PostAsJsonAsync("/academic/course-curriculums", data);
        return await response.Resolve<CourseCurriculumOut>();
    }

    public async Task<List<CourseCurriculumOut>> GetCourseCurriculums()
    {
        return await Http.GetFromJsonAsync<List<CourseCurriculumOut>>("/academic/course-curriculums", HttpConfigs.JsonOptions) ?? [];
    }

    public async Task<OneOf<CourseOfferingOut, ErrorOut>> CreateCourseOffering(
        Guid campusId,
        Guid courseId,
        Guid courseCurriculumId,
        string? period,
        Shift? shift
    ) {
        var data = new CreateCourseOfferingIn
        {
            CampusId = campusId,
            CourseId = courseId,
            CourseCurriculumId = courseCurriculumId,
            Period = period,
            Shift = shift,
        };
        var response = await Http.PostAsJsonAsync("/academic/course-offerings", data);
        return await response.Resolve<CourseOfferingOut>();
    }

    public async Task<List<CourseOfferingOut>> GetCourseOfferings()
    {
        return await Http.GetFromJsonAsync<List<CourseOfferingOut>>("/academic/course-offerings", HttpConfigs.JsonOptions) ?? [];
    }

    public async Task<AcademicInsightsOut> GetAcademicInsights()
    {
        return await Http.GetFromJsonAsync<AcademicInsightsOut>("/academic/insights", HttpConfigs.JsonOptions) ?? new();
    }

    public async Task<OneOf<TeacherOut, ErrorOut>> CreateTeacher(
        string name = "Chico",
        string email = null
    ) {
        email ??= TestData.Email;
        var data = new CreateTeacherIn { Name = name, Email = email };
        var response = await Http.PostAsJsonAsync("/academic/teachers", data);
        var result = await response.Resolve<TeacherOut>();

        if (result.IsSuccess)
        {
            var teacher = result.Success;
            teacher.Email = email;
            return teacher;
        }

        return result.Error;
    }

    public async Task<List<TeacherOut>> GetTeachers()
    {
        return await Http.GetFromJsonAsync<List<TeacherOut>>("/academic/teachers", HttpConfigs.JsonOptions) ?? [];
    }

    public async Task<OneOf<ClassOut, ErrorOut>> CreateClass(
        Guid disciplineId,
        Guid? campusId,
        Guid? teacherId,
        string period,
        int vacancies,
        List<ScheduleIn> schedules
    ) {
        var data = new CreateClassIn(disciplineId, campusId, teacherId, period, vacancies, schedules);
        var response = await Http.PostAsJsonAsync("/academic/classes", data);
        return await response.Resolve<ClassOut>();
    }

    public async Task<OneOf<CreateClassroomOut, ErrorOut>> CreateClassroom(
        Guid campusId,
        string name,
        int capacity
    ) {
        var data = new CreateClassroomIn { CampusId = campusId, Name = name, Capacity = capacity };
        var response = await Http.PostAsJsonAsync("/academic/classrooms", data);
        return await response.Resolve<CreateClassroomOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> ReleaseClassesForEnrollment(List<Guid> classes)
    {
        var data = new ReleaseClassesForEnrollmentIn { Classes = classes };
        var response = await Http.PutAsJsonAsync("/academic/classes/release-for-enrollment", data);
        return await response.Resolve<SuccessOut>();
    }

    public async Task<List<ClassOut>> GetAcademicClasses(GetAcademicClassesIn query = null)
    {
        var queryString = query?.Status != null ? $"?status={query.Status}" : "";
        return await Http.GetFromJsonAsync<List<ClassOut>>($"/academic/classes{queryString}", HttpConfigs.JsonOptions) ?? [];
    }

    public async Task<OneOf<GetAcademicClassOut, ErrorOut>> GetAcademicClass(Guid id)
    {
        var response = await Http.GetAsync($"/academic/classes/{id}");
        return await response.Resolve<GetAcademicClassOut>();
    }

    public async Task<OneOf<StudentOut, ErrorOut>> CreateStudent(
        Guid courseOfferingId,
        string name = "Zezin",
        string email = null,
        string? phoneNumber = null
    ) {
        email ??= TestData.Email;
        var data = new CreateStudentIn
        {
            Name = name,
            Email = email,
            PhoneNumber = phoneNumber,
            CourseOfferingId = courseOfferingId,
        };
        var response = await Http.PostAsJsonAsync("/academic/students", data);
        var result = await response.Resolve<StudentOut>();

        if (result.IsSuccess)
        {
            var student = result.Success;
            student.Email = email;
            return student;
        }

        return result.Error;
    }

    public async Task<List<StudentOut>> GetStudents()
    {
        return await Http.GetFromJsonAsync<List<StudentOut>>("/academic/students", HttpConfigs.JsonOptions) ?? [];
    }

    public async Task<OneOf<AcademicPeriodOut, ErrorOut>> CreateAcademicPeriod(
        string id,
        DateOnly? startAt = null,
        DateOnly? endAt = null
    ) {
        if (startAt == null || endAt == null)
        {
            var periodIn = new CreateAcademicPeriodIn(id);
            startAt = periodIn.StartAt;
            endAt = periodIn.EndAt;
        }

        var data = new CreateAcademicPeriodIn { Id = id, StartAt = startAt.Value, EndAt = endAt.Value };
        var response = await Http.PostAsJsonAsync("/academic/academic-periods", data);
        return await response.Resolve<AcademicPeriodOut>();
    }

    public async Task<List<AcademicPeriodOut>> GetAcademicPeriods()
    {
        return await Http.GetFromJsonAsync<List<AcademicPeriodOut>>("/academic/academic-periods", HttpConfigs.JsonOptions) ?? [];
    }

    public async Task<OneOf<EnrollmentPeriodOut, ErrorOut>> CreateEnrollmentPeriod(string id, int start = -2, int end = 2)
    {
        var startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(start));
        var endDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(end));
        var data = new CreateEnrollmentPeriodIn { Id = id, StartAt = startDate, EndAt = endDate };
        var response = await Http.PostAsJsonAsync("/academic/enrollment-periods", data);
        return await response.Resolve<EnrollmentPeriodOut>();
    }

    public async Task<OneOf<EnrollmentPeriodOut, ErrorOut>> UpdateEnrollmentPeriod(string id, int start = -4, int end = 4)
    {
        var startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(start));
        var endDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(end));
        var data = new UpdateEnrollmentPeriodIn { StartAt = startDate, EndAt = endDate };
        var response = await Http.PutAsJsonAsync($"/academic/enrollment-periods/{id}", data);
        return await response.Resolve<EnrollmentPeriodOut>();
    }

    public async Task<List<EnrollmentPeriodOut>> GetEnrollmentPeriods()
    {
        return await Http.GetFromJsonAsync<List<EnrollmentPeriodOut>>("/academic/enrollment-periods", HttpConfigs.JsonOptions) ?? [];
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> StartClasses(List<Guid> classes)
    {
        var data = new StartClassesIn { Classes = classes };
        var response = await Http.PutAsJsonAsync("/academic/classes/start", data);
        return await response.Resolve<SuccessOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> FinalizeClasses(List<Guid> classes)
    {
        var data = new FinalizeClassesIn { Classes = classes };
        var response = await Http.PutAsJsonAsync("/academic/classes/finalize", data);
        return await response.Resolve<SuccessOut>();
    }

    public async Task<OneOf<CreateWebhookSubscriptionOut, ErrorOut>> CreateWebhookSubscription(
        string name = "Aluno Criado",
        string url = "https://example.com/webhook",
        List<WebhookEventType> events = null,
        WebhookAuthenticationType authenticationType = WebhookAuthenticationType.ApiKey,
        string? apiKey = "z3Q6uDUJYTDCIo16myBKZrlCS63IvpCUOAE5X"
    ) {
        events ??= [WebhookEventType.StudentCreated];
        var data = new CreateWebhookSubscriptionIn
        {
            Name = name,
            Url = url,
            Events = events,
            AuthenticationType = authenticationType,
            ApiKey = apiKey,
        };
        var response = await Http.PostAsJsonAsync("/academic/webhooks", data);
        return await response.Resolve<CreateWebhookSubscriptionOut>();
    }

    public async Task<OneOf<GetWebhookSubscriptionOut, EstudError>> GetWebhookSubscription(Guid id)
    {
        return await Http.GetFromJsonAsync<GetWebhookSubscriptionOut>($"/academic/webhooks/{id}", HttpConfigs.JsonOptions) ?? new();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> AddDisciplinePreRequisites(
        Guid courseCurriculumId,
        Guid disciplineId,
        List<Guid> preRequisites
    ) {
        var data = new AddDisciplinePreRequisitesIn { DisciplineId = disciplineId, PreRequisites = preRequisites };
        var response = await Http.PostAsJsonAsync($"/academic/course-curriculums/{courseCurriculumId}/pre-requisites", data);
        return await response.Resolve<SuccessOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> AssignDisciplinesToTeacher(Guid teacherId, List<Guid> classes)
    {
        var data = new AssignDisciplinesToTeacherIn { Disciplines = classes };
        var response = await Http.PutAsJsonAsync($"/academic/teachers/{teacherId}/assign-disciplines", data);
        return await response.Resolve<SuccessOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> AssignCampiToTeacher(Guid teacherId, List<Guid> campi)
    {
        var data = new AssignCampiToTeacherIn { Campi = campi };
        var response = await Http.PutAsJsonAsync($"/academic/teachers/{teacherId}/assign-campi", data);
        return await response.Resolve<SuccessOut>();
    }

    public async Task<OneOf<SuccessOut, ErrorOut>> AssignClassToClassroom(Guid classroomId, Guid classId, List<ScheduleIn> schedules)
    {
        var data = new AssignClassToClassroomIn { ClassId = classId, Schedules = schedules };
        var response = await Http.PutAsJsonAsync($"/academic/classrooms/{classroomId}/assign-class", data);
        return await response.Resolve<SuccessOut>();
    }

    public async Task<AcademicPeriodOut> CreateCurrentAcademicPeriod()
    {
        var today = DateTime.UtcNow;
        var number = today.Month <= 6 ? 1 : 2;
        var startAt = today.AddDays(-30).ToDateOnly();
        var endAt = today.AddDays(3).ToDateOnly();
        return await CreateAcademicPeriod($"{today.Year}.{number}", startAt, endAt);
    }

    public async Task<BasicInstitutionTestDto> CreateAdsCourseOffering(
        BasicInstitutionTestDto data = null,
        Guid? campusId = null,
        string periodId = null)
    {
        data ??= new BasicInstitutionTestDto();

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

        data.AdsDisciplines.DiscreteMath = data.AdsCourse.Disciplines.Single(x => x.Name == "Matemática Discreta");
        data.AdsDisciplines.IntroToWebDev = data.AdsCourse.Disciplines.Single(x => x.Name == "Introdução ao Desenvolvimento Web");
        data.AdsDisciplines.HumanMachineInteractionDesign = data.AdsCourse.Disciplines.Single(x => x.Name == "Design de Interação Humano-Máquina");
        data.AdsDisciplines.IntroToComputerNetworks = data.AdsCourse.Disciplines.Single(x => x.Name == "Introdução à Redes de Computadores");
        data.AdsDisciplines.ComputationalThinkingAndAlgorithms = data.AdsCourse.Disciplines.Single(x => x.Name == "Pensamento Computacional e Algoritmos");
        data.AdsDisciplines.IntegratorProjectOne = data.AdsCourse.Disciplines.Single(x => x.Name == "Projeto Integrador I: Concepção e Prototipação");

        data.AdsDisciplines.Arch = data.AdsCourse.Disciplines.Single(x => x.Name == "Arquitetura de Computadores e Sistemas Operacionais");
        data.AdsDisciplines.Databases = data.AdsCourse.Disciplines.Single(x => x.Name == "Banco de Dados");
        data.AdsDisciplines.DataStructures = data.AdsCourse.Disciplines.Single(x => x.Name == "Estrutura de Dados");
        data.AdsDisciplines.InfoAndSociety = data.AdsCourse.Disciplines.Single(x => x.Name == "Informática e Sociedade");
        data.AdsDisciplines.Poo = data.AdsCourse.Disciplines.Single(x => x.Name == "Programação Orientada a Objetos");
        data.AdsDisciplines.IntegratorProjectTwo = data.AdsCourse.Disciplines.Single(x => x.Name == "Projeto Integrador II: Modelagem de Banco de Dados");

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

        data.AdsCourseOffering = await CreateCourseOffering(campusId ?? data.Campus.Id, data.AdsCourse.Id, data.AdsCourseCurriculum.Id, periodId ?? data.AcademicPeriod2.Id, Shift.Noturno);
        return data;
    }

    public async Task<BasicInstitutionTestDto> CreateBasicInstitutionData()
    {
        var data = new BasicInstitutionTestDto();

        data.AcademicPeriod1 = await CreateAcademicPeriod($"{DateTime.UtcNow.Year}.1");
        data.AcademicPeriod2 = await CreateAcademicPeriod($"{DateTime.UtcNow.Year}.2");

        data.Campus = await CreateCampus();

        // Ads
        await CreateAdsCourseOffering(data);

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

        data.DireitoDisciplines.PhilosophicalBases = data.DireitoCourse.Disciplines.Single(x => x.Name == "Bases Filosóficas");
        data.DireitoDisciplines.CommunicationAndLegalArgumentation = data.DireitoCourse.Disciplines.Single(x => x.Name == "Comunicação e Argumentação Jurídica");
        data.DireitoDisciplines.ManSocietyAndLaw = data.DireitoCourse.Disciplines.Single(x => x.Name == "Homem, Sociedade e Direito");
        data.DireitoDisciplines.PoliticsAndStateInFocus = data.DireitoCourse.Disciplines.Single(x => x.Name == "Política e Estado em Foco");
        data.DireitoDisciplines.GeneralTheoryOfLaw = data.DireitoCourse.Disciplines.Single(x => x.Name == "Teoria Geral do Direito");

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
        await AssignCampiToTeacher(data.Teacher.Id, [data.Campus.Id]);
        await AssignDisciplinesToTeacher(data.Teacher.Id, data.AdsDisciplines.GetIds());

        data.AdsClasses.HumanMachineInteractionDesign = await CreateClass(data.AdsDisciplines.HumanMachineInteractionDesign.Id, data.Campus.Id, data.Teacher.Id, period, 45, [new(Day.Monday, Hour.H07_00, Hour.H10_00)]);
        data.AdsClasses.IntroToComputerNetworks = await CreateClass(data.AdsDisciplines.IntroToComputerNetworks.Id, data.Campus.Id, data.Teacher.Id, period, 45, [new(Day.Tuesday, Hour.H07_00, Hour.H10_00)]);
        data.AdsClasses.IntroToWebDev = await CreateClass(data.AdsDisciplines.IntroToWebDev.Id, data.Campus.Id, data.Teacher.Id, period, 45, [new(Day.Wednesday, Hour.H07_00, Hour.H10_00)]);
        data.AdsClasses.DiscreteMath = await CreateClass(data.AdsDisciplines.DiscreteMath.Id, data.Campus.Id, data.Teacher.Id, period, 40, [new(Day.Thursday, Hour.H07_00, Hour.H10_00)]);
        data.AdsClasses.ComputationalThinkingAndAlgorithms = await CreateClass(data.AdsDisciplines.ComputationalThinkingAndAlgorithms.Id, data.Campus.Id, data.Teacher.Id, period, 45, [new(Day.Friday, Hour.H07_00, Hour.H10_00)]);
        data.AdsClasses.IntegratorProjectOne = await CreateClass(data.AdsDisciplines.IntegratorProjectOne.Id, data.Campus.Id, data.Teacher.Id, period, 45, [new(Day.Saturday, Hour.H07_00, Hour.H10_00)]);

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
