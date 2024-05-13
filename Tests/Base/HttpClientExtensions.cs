using Syki.Tests.Mock;
using Syki.Front.Auth;
using System.Net.Http.Json;
using Syki.Front.Features.Cross.Login;
using Syki.Front.Features.Cross.SetupMfa;
using Syki.Front.Features.Cross.LoginMfa;
using Syki.Front.Features.Cross.GetMfaKey;
using Syki.Front.Features.Cross.ResetPassword;
using Syki.Front.Features.Academic.GetCourses;
using Syki.Front.Features.Academic.CreateCampus;
using Syki.Front.Features.Academic.UpdateCampus;
using Syki.Front.Features.Academic.CreateCourse;
using Syki.Front.Features.Academic.CreateTeacher;
using Syki.Front.Features.Academic.GetDisciplines;
using Syki.Front.Features.Cross.FinishUserRegister;
using Syki.Front.Features.Academic.CreateDiscipline;
using Syki.Front.Features.Academic.GetAcademicPeriods;
using Syki.Front.Features.Cross.SendResetPasswordToken;
using Syki.Front.Features.Academic.GetAcademicInsights;
using Syki.Front.Features.Academic.CreateAcademicPeriod;
using Syki.Front.Features.Academic.CreateEnrollmentPeriod;
using Syki.Front.Features.Academic.CreateCourseCurriculum;
using Syki.Front.Features.Cross.CreatePendingUserRegister;
using Syki.Front.Features.Academic.CreateNotification;
using Syki.Front.Features.Academic.GetCampi;

namespace Syki.Tests.Base;

public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage> CreatePendingUserRegister(this HttpClient http, string email)
    {
        var client = new CreatePendingUserRegisterClient(http);
        return await client.Create(email);
    }

    public static async Task<HttpResponseMessage> FinishUserRegister(this HttpClient http, string token, string password)
    {
        var client = new FinishUserRegisterClient(http);
        return await client.Finish(token, password);
    }

    public static async Task<UserOut> RegisterUser(this HttpClient client, BackWebApplicationFactory factory)
    {
        var email = TestData.Email;
        var password = "Lalala@123";

        await client.CreatePendingUserRegister(email);
        var token = await factory.GetRegisterSetupToken(email);

        await client.FinishUserRegister(token!, password);

        return new UserOut { Email = email, Password = password };
    }

    public static async Task<LoginOut> Login(this HttpClient http, string email, string password)
    {
        var storage = new LocalStorageServiceMock();
        var client = new LoginClient(http, storage, new SykiAuthStateProvider(storage));
        var response = await client.Login(email, password);

        http.RemoveAuthToken();
        http.AddAuthToken(response.AccessToken);

        return response;
    }

    public static async Task<GetMfaKeyOut> GetMfaKey(this HttpClient http)
    {
        var client = new GetMfaKeyClient(http);
        return await client.Get();
    }

    public static async Task<bool> SetupMfa(this HttpClient http, string token)
    {
        var client = new SetupMfaClient(http);
        return await client.Setup(token);
    }

    public static async Task<LoginMfaOut> LoginMfa(this HttpClient http, string token)
    {
        var storage=  new LocalStorageServiceMock();
        var client = new LoginMfaClient(http, storage, new SykiAuthStateProvider(storage));
        var response = await client.Login(token);

        http.RemoveAuthToken();
        http.AddAuthToken(response.AccessToken);

        return response;
    }

    public static async Task<HttpResponseMessage> SendResetPasswordToken(this HttpClient http, string email)
    {
        var client = new SendResetPasswordTokenClient(http);
        return await client.Send(email);
    }

    public static async Task<HttpResponseMessage> ResetPassword(this HttpClient http, string token, string password)
    {
        var client = new ResetPasswordClient(http);
        return await client.Reset(token, password);
    }

    public static void RemoveAuthToken(this HttpClient client)
    {
        client.DefaultRequestHeaders.Remove("Authorization");
    }

    public static void AddAuthToken(this HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }

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

    public static async Task<DisciplineOut> CreateDiscipline(
        this HttpClient http,
        string name = "Banco de Dados",
        List<Guid> courses = null
    ) {
        var client = new CreateDisciplineClient(http);
        var response = await client.Create(name, courses ?? []);
        return await response.DeserializeTo<DisciplineOut>();
    }

    // -------------------------------------------------------------------------------------------- //












    public static async Task<AcademicInsightsOut> GetAcademicInsights(this HttpClient http)
    {
        var client = new GetAcademicInsightsClient(http);
        return await client.Get();
    }

    public static async Task<CampusOut> UpdateCampus(this HttpClient http, Guid id, string name = "Agreste I", string city = "Caruaru - PE")
    {
        var client = new UpdateCampusClient(http);
        var response = await client.Update(id, name, city);
        return await response.DeserializeTo<CampusOut>();
    }

    public static async Task<CourseOut> CreateCourse(
        this HttpClient http,
        string name = "Análise e Desenvolvimento de Sistemas",
        CourseType tipo = CourseType.Bacharelado
    ) {
        var client = new CreateCourseClient(http);
        var response = await client.Create(name, tipo);
        return await response.DeserializeTo<CourseOut>();
    }

    public static async Task<List<CourseOut>> GetCursos(this HttpClient http)
    {
        var client = new GetCoursesClient(http);
        return await client.Get();
    }



    public static async Task<TeacherOut> CreateProfessor(
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

    public static async Task<ClassOut> Createturma(
        this HttpClient http,
        Guid disciplineId,
        Guid professorId,
        string periodoId,
        List<ScheduleIn> schedules
    ) {
        var body = new CreateClassIn(disciplineId, professorId, periodoId, schedules);
        return await http.PostAsync<ClassOut>("/turmas", body);
    }

    public static async Task<HttpResponseMessage> CreateturmaHttp(
        this HttpClient http,
        Guid disciplineId,
        Guid professorId,
        string periodoId,
        List<ScheduleIn> schedules
    ) {
        var body = new CreateClassIn(disciplineId, professorId, periodoId, schedules);
        return await http.PostAsJsonAsync("/turmas", body);
    }

    public static async Task<CourseCurriculumOut> CreateGrade(
        this HttpClient http,
        string name,
        Guid cursoId,
        List<CreateCourseCurriculumDisciplineIn> disciplines = null
    ) {
        var client = new CreateCourseCurriculumClient(http);

        var result = await client.Create(name, cursoId, disciplines ?? []);

        return await result.DeserializeTo<CourseCurriculumOut>();
    }

    public static async Task<HttpResponseMessage> CreateGradeHttp(
        this HttpClient http,
        string name,
        Guid cursoId,
        List<CreateCourseCurriculumDisciplineIn> disciplines = null
    ) {
        var client = new CreateCourseCurriculumClient(http);

        return await client.Create(name, cursoId, disciplines ?? []);
    }

    public static async Task<CourseOfferingOut> CreateOferta(
        this HttpClient http,
        Guid campusId,
        Guid cursoId,
        Guid courseCurriculumId,
        string? period,
        Shift shift
    ) {
        var body = new CreateCourseOfferingIn
        {
            CampusId = campusId,
            CourseId = cursoId,
            CourseCurriculumId = courseCurriculumId,
            Period = period,
            Shift = shift,
        };

        return await http.PostAsync<CourseOfferingOut>("/ofertas", body);
    }

    public static async Task<HttpResponseMessage> CreateOfertaHttp(
        this HttpClient http,
        Guid campusId,
        Guid cursoId,
        Guid courseCurriculumId,
        string? period,
        Shift shift
    ) {
        var body = new CreateCourseOfferingIn
        {
            CampusId = campusId,
            CourseId = cursoId,
            CourseCurriculumId = courseCurriculumId,
            Period = period,
            Shift = shift,
        };

        return await http.PostAsJsonAsync("/ofertas", body);
    }

    public static async Task<StudentOut> CreateStudent(
        this HttpClient http,
        Guid ofertaId,
        string name = "Zezin",
        string email = ""
    ) {
        email = email.HasValue() ? email : TestData.Email;

        var body = new CreateStudentIn { Name = name, Email = email, CourseOfferingId = ofertaId };

        var aluno = await http.PostAsync<StudentOut>("/alunos", body);
        aluno.Email = email;
        return aluno;
    }

    public static async Task<HttpResponseMessage> CreateAlunoHttp(
        this HttpClient http,
        Guid ofertaId,
        string name = "Zezin",
        string email = ""
    ) {
        email = email.HasValue() ? email : TestData.Email;

        var body = new CreateStudentIn { Name = name, Email = email, CourseOfferingId = ofertaId };

        return await http.PostAsJsonAsync("/alunos", body);
    }

    public static async Task<List<DisciplineOut>> GetAlunoDisciplines(this HttpClient http)
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

    public static async Task<EnrollmentPeriodOut> GetCurrentEnrollmentPeriod(this HttpClient http)
    {
        var client = new GetCurrentEnrollmentPeriodClient(http);
        return await client.Get();
    }


















    public static async Task PostAsync(this HttpClient client, string path, object obj)
    {
        await client.PostAsync(path, obj.ToStringContent());
    }

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

    public static async Task<string> ResetPassword(this HttpClient client, string token)
    {
        var bodyReset = new ResetPasswordIn { Token = token, Password = "My@newP4sswordMy@newP4ssword" };

        await client.PostAsync("/reset-password", bodyReset);

        return bodyReset.Password;
    }
}
