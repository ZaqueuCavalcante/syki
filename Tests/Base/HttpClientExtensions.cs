using Syki.Shared;
using Syki.Shared.Login;
using Syki.Shared.CreateBook;
using Syki.Shared.CreateUser;
using static Syki.Back.Configs.AuthorizationConfigs;
using Syki.Shared.CreateCampus;

namespace Syki.Tests.Base;

public static class HttpClientExtensions
{
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

    public static async Task<string> Login(this HttpClient client, string email, string password)
    {
        var data = new LoginIn { Email = email, Password = password };
        var response = await client.PostAsync<LoginOut>("/login", data);

        client.RemoveAuthToken();
        client.AddAuthToken(response.AccessToken);

        return response.AccessToken;
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

    public static async Task<BookOut> CreateBook(
        this HttpClient client,
        string title = "Manual de DevOps"
    ) {
        var body = new CreateBookIn { Title = "Manual de DevOps" };
        return await client.PostAsync<BookOut>("/books", body);
    }

    public static async Task<CampusOut> NewCampus(
        this HttpClient client,
        string name = "Agreste I",
        string city = "Caruaru - PE"
    ) {
        var body = new CreateCampusIn { Name = name, City = city };
        return await client.PostAsync<CampusOut>("/campi", body);
    }

    public static async Task<CursoOut> NewCurso(
        this HttpClient client,
        string nome = "Análise e Desenvolvimento de Sistemas",
        TipoDeCurso tipo = TipoDeCurso.Bacharelado
    ) {
        var body = new CursoIn { Nome = nome, Tipo = tipo };
        return await client.PostAsync<CursoOut>("/cursos", body);
    }

    public static async Task<DisciplinaOut> NewDisciplina(
        this HttpClient client,
        string nome = "Banco de Dados",
        List<Guid> cursos = null
    ) {
        var body = new DisciplinaIn { Nome = nome, Cursos = cursos ?? [] };
        return await client.PostAsync<DisciplinaOut>("/disciplinas", body);
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

    public static async Task<PeriodoOut> NewPeriodo(
        this HttpClient client,
        string id
    ) {
        return await client.PostAsync<PeriodoOut>("/periodos", new PeriodoIn(id));
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

        await client.PostAsync<ResetPasswordOut>("/users/reset-password", bodyReset);

        return bodyReset.Password;
    }
}
