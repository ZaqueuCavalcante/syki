using Syki.Back.Configs;

namespace Syki.Tests.Base;

[Category("Auth")]
public class AuthTestBase
{
    protected HttpClient _client = null!;
    protected BackWebApplicationFactory _factory = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _factory = new BackWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        _client.Dispose();
        await _factory.DisposeAsync();
    }

    protected void Login(string role)
    {
        var jwt = role switch
        {
            AuthorizationConfigs.Adm => JWTAdm,
            AuthorizationConfigs.Academico => JWTAcademico,
            AuthorizationConfigs.Professor => JWTProfessor,
            AuthorizationConfigs.Aluno => JWTAluno,
            _ => ""
        };
        _client.RemoveAuthToken();
        _client.AddAuthToken(jwt);
    }

    protected void Logout()
    {
        _client.RemoveAuthToken();
    }

    public const string JWTAdm = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiQWRtIiwibmJmIjoxNzA0NTA1MDI3LCJleHAiOjUzMDQ1MDUwMjcsImlhdCI6MTcwNDUwNTAyNywiaXNzIjoic3lraS1hcGktdGVzdGluZyIsImF1ZCI6InN5a2ktYXBpLXRlc3RpbmcifQ.7tEWvPsGJdTUEsjQWAFMi7RqoVRmpZfeRbVuvQTsFLM";
    public const string JWTAcademico = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIyZGIwNTRiNS04MGY5LTQwM2UtOGJiYS05M2EzNzQ2Mjk4NDciLCJmYWN1bGRhZGUiOiIwMDAwMDAwMC0wMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDAiLCJyb2xlIjoiQWNhZGVtaWNvIiwibmJmIjoxNzA0NTA2NTkwLCJleHAiOjUzMDQ1MDY1OTAsImlhdCI6MTcwNDUwNjU5MCwiaXNzIjoic3lraS1hcGktdGVzdGluZyIsImF1ZCI6InN5a2ktYXBpLXRlc3RpbmcifQ.h-flkIm6Y7L4xI-HovMFHOpgMejvkQwrsMynI2VWDMY";
    public const string JWTProfessor = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJlOWYyYjkxYi01NzYzLTRmNjUtYWY5Zi1kMzM4NzBmM2I2MTYiLCJmYWN1bGRhZGUiOiIwMDAwMDAwMC0wMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDAiLCJyb2xlIjoiUHJvZmVzc29yIiwibmJmIjoxNzA0NTA2OTQ4LCJleHAiOjUzMDQ1MDY5NDgsImlhdCI6MTcwNDUwNjk0OCwiaXNzIjoic3lraS1hcGktdGVzdGluZyIsImF1ZCI6InN5a2ktYXBpLXRlc3RpbmcifQ.PRkkjr-xj8DkUumwAOY0Sqx-5p4eecYeFn4fv963OC8";
    public const string JWTAluno = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI0OTA1MmE4NC1mYTMxLTQ1MTEtODk1OS1iZTg0NWZjMDIyOTgiLCJmYWN1bGRhZGUiOiIwMDAwMDAwMC0wMDAwLTAwMDAtMDAwMC0wMDAwMDAwMDAwMDAiLCJyb2xlIjoiQWx1bm8iLCJuYmYiOjE3MDQ1MDY5ODQsImV4cCI6NTMwNDUwNjk4NCwiaWF0IjoxNzA0NTA2OTg0LCJpc3MiOiJzeWtpLWFwaS10ZXN0aW5nIiwiYXVkIjoic3lraS1hcGktdGVzdGluZyJ9.5jJnjjDLk5sZ-WzN9FeNuFwmpEAuXutQCJGgmbTrMFM";
}
