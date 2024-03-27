using Syki.Front.SetupMfa;
using Syki.Front.GetMfaKey;
using Syki.Front.FinishUserRegister;
using Syki.Front.CreateAcademicPeriod;
using Syki.Front.CreateEnrollmentPeriod;
using Syki.Front.CreatePendingUserRegister;
using static Syki.Back.Configs.AuthorizationConfigs;
using Syki.Front.Login;
using Syki.Tests.Mock;
using Syki.Front.Auth;
using Syki.Front.LoginMfa;
using Syki.Front.SendResetPasswordToken;
using Syki.Front.ResetPassword;
using Front.CreateCampus;
using Front.UpdateCampus;
using Front.CreateCurso;
using Front.GetCursos;
using Front.CreateDisciplina;
using Front.CreateEvaluationUnits;
using Front.CreateProfessor;

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

    public static async Task<CreateUserOut> RegisterUser(this HttpClient client, BackWebApplicationFactory factory)
    {
        var email = TestData.Email;
        var password = "Lalala@123";

        await client.CreatePendingUserRegister(email);
        var token = await factory.GetRegisterSetupToken(email);

        await client.FinishUserRegister(token!, password);

        return new CreateUserOut { Email = email, Password = password };
    }

    public static async Task<LoginOut> Login(this HttpClient http, string email, string password)
    {
        var storage=  new LocalStorageServiceMock();
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

    public static async Task<bool> SetupMfa(this HttpClient http, string code)
    {
        var client = new SetupMfaClient(http);
        return await client.Setup(code);
    }

    public static async Task<LoginMfaOut> LoginMfa(this HttpClient http, string code)
    {
        var storage=  new LocalStorageServiceMock();
        var client = new LoginMfaClient(http, storage, new SykiAuthStateProvider(storage));
        var response = await client.Login(code);

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

    public static async Task<CampusOut> CreateCampus(this HttpClient http, string name = "Agreste I", string city = "Caruaru - PE")
    {
        var client = new CreateCampusClient(http);
        var response = await client.Create(name, city);
        return await response.DeserializeTo<CampusOut>();
    }

    public static async Task<CampusOut> UpdateCampus(this HttpClient http, Guid id, string name = "Agreste I", string city = "Caruaru - PE")
    {
        var client = new UpdateCampusClient(http);
        var response = await client.Update(id, name, city);
        return await response.DeserializeTo<CampusOut>();
    }

    public static async Task<CursoOut> CreateCurso(
        this HttpClient http,
        string nome = "Análise e Desenvolvimento de Sistemas",
        TipoDeCurso tipo = TipoDeCurso.Bacharelado
    ) {
        var client = new CreateCursoClient(http);
        var response = await client.Create(nome, tipo);
        return await response.DeserializeTo<CursoOut>();
    }

    public static async Task<List<CursoOut>> GetCursos(this HttpClient http)
    {
        var client = new GetCursosClient(http);
        return await client.Get();
    }

    public static async Task<DisciplinaOut> CreateDisciplina(
        this HttpClient http,
        string nome = "Banco de Dados",
        List<Guid> cursos = null
    ) {
        var client = new CreateDisciplinaClient(http);
        var response = await client.Create(nome, cursos ?? []);
        return await response.DeserializeTo<DisciplinaOut>();
    }

    public static async Task<ProfessorOut> CreateProfessor(
        this HttpClient http,
        string name = "Chico",
        string email = ""
    ) {
        email = email.HasValue() ? email : TestData.Email;
        var client = new CreateProfessorClient(http);
        var response = await client.Create(name, email);
        var professor= await response.DeserializeTo<ProfessorOut>();
        professor.Email = email;
        return professor;
    }

    public static async Task<TurmaOut> Createturma(
        this HttpClient http,
        Guid disciplinaId,
        Guid professorId,
        string periodoId,
        List<HorarioIn> horarios
    ) {
        var body = new TurmaIn(disciplinaId, professorId, periodoId, horarios);
        return await http.PostAsync<TurmaOut>("/turmas", body);
    }

    public static async Task<HttpResponseMessage> CreateEvaluationUnits(
        this HttpClient http,
        Guid turmaId,
        List<EvaluationUnitIn> units
    ) {
        var client = new CreateEvaluationUnitsClient(http);
        return await client.Create(turmaId, units ?? []);
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










    public static async Task<CreateUserOut> RegisterUser(this HttpClient client, CreateUserIn body)
    {
        var response = await client.PostHttpAsync("/users", body);
        return await response.DeserializeTo<CreateUserOut>();
    }

    public static async Task<CreateUserIn> NewAcademico(this HttpClient client, string faculdade)
    {
        var novaRoma = await client.CreateInstitution(faculdade);
        var userNovaRoma = CreateUserIn.New(novaRoma.Id, Academico);
        await client.RegisterUser(userNovaRoma);
        return userNovaRoma;
    }

    public static async Task LoginAsAdm(this HttpClient client)
    {
        await client.Login("adm@syki.com", "Admin@123Admin@123");
    }

    public static async Task Login(this HttpClient client, CreateUserIn user)
    {
        await client.Login(user.Email, user.Password);
    }

    public static async Task<FaculdadeOut> CreateInstitution(this HttpClient client, string nome = "Nova Roma")
    {
        await client.LoginAsAdm();

        var body = new FaculdadeIn { Nome = nome };
        var response = await client.PostHttpAsync("/faculdades", body);

        return await response.DeserializeTo<FaculdadeOut>();
    }

    public static async Task<CreateUserIn> RegisterAndLogin(this HttpClient client, Guid faculdadeId, string role)
    {
        if (role != "Adm")
        {
            var user = CreateUserIn.New(faculdadeId, role);
            await client.RegisterUser(user);
            await client.Login(user.Email, user.Password);
            return user;
        }

        return null!;
    }







    public static async Task<GradeOut> NewGrade(
        this HttpClient client,
        string nome,
        Guid cursoId,
        List<GradeDisciplinaIn> disciplinas = null
    ) {
        var body = new GradeIn {
            Nome = nome,
            CursoId = cursoId,
            Disciplinas = disciplinas ?? []
        };

        return await client.PostAsync<GradeOut>("/grades", body);
    }

    public static async Task<OfertaOut> NewOferta(
        this HttpClient client,
        Guid campusId,
        Guid cursoId,
        Guid gradeId,
        string? periodo,
        Turno turno = Turno.Noturno
    ) {
        var body = new OfertaIn {
            CampusId = campusId,
            Periodo = periodo,
            CursoId = cursoId,
            GradeId = gradeId,
            Turno = turno,
        };

        return await client.PostAsync<OfertaOut>("/ofertas", body);
    }

    public static void RemoveAuthToken(this HttpClient client)
    {
        client.DefaultRequestHeaders.Remove("Authorization");
    }

    public static void AddAuthToken(this HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
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

    public static async Task<T> PutAsync<T>(this HttpClient client, string path, object obj)
    {
        var response = await client.PutAsync(path, obj.ToStringContent());
        return await response.DeserializeTo<T>();
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
