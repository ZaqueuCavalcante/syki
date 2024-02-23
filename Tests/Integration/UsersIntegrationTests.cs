using Syki.Shared;
using Syki.Shared.CreateUser;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Nao_deve_logar_quando_informar_email_e_senha_corretos_mas_precisa_de_mfa_e_informa_codigo_errado()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        var user = await client.RegisterAndLogin(faculdade.Id, Academico);

        var keyResponse = await client.GetAsync<MfaKeyOut>("/users/mfa-key");
        var token = keyResponse.Key.ToMfaToken();
        await client.PostAsync<MfaSetupOut>("/users/mfa-setup", new MfaSetupIn { Token = token });

        client.RemoveAuthToken();

        var data = new LoginIn { Email = user.Email, Password = user.Password };
        await client.PostAsync("/login", data.ToStringContent());

        var body = new LoginMfaIn { Code = Guid.NewGuid().ToHashCode().ToString()[..6] };

        // Act
        var response = await client.PostAsync("/users/login-mfa", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<LoginOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Wrong2FactorCode.Should().BeTrue();
    }

    [Test]
    public async Task Nao_deve_logar_quando_tentar_obter_o_jwt_sem_informar_email_e_senha_antes()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var keyResponse = await client.GetAsync<MfaKeyOut>("/users/mfa-key");
        var token = keyResponse.Key.ToMfaToken();
        await client.PostAsync<MfaSetupOut>("/users/mfa-setup", new MfaSetupIn { Token = token });

        client.RemoveAuthToken();

        var body = new LoginMfaIn { Code = token };

        // Act
        var response = await client.PostAsync("/users/login-mfa", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<LoginOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Wrong2FactorCode.Should().BeTrue();
    }

    [Test]
    public async Task Deve_logar_quando_informar_email_e_senha_corretos_mas_precisa_de_mfa_e_informa_codigo_correto()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        var user = await client.RegisterAndLogin(faculdade.Id, Academico);

        var keyResponse = await client.GetAsync<MfaKeyOut>("/users/mfa-key");
        var token = keyResponse.Key.ToMfaToken();
        await client.PostAsync<MfaSetupOut>("/users/mfa-setup", new MfaSetupIn { Token = token });

        client.RemoveAuthToken();

        var data = new LoginIn { Email = user.Email, Password = user.Password };
        await client.PostHttpAsync("/login", data);

        var body = new LoginMfaIn { Code = token };

        // Act
        var response = await client.PostAsync<LoginOut>("/users/login-mfa", body);

        // Assert
        response.AccessToken.Should().StartWith("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.");
    }

    [Test]
    public async Task Deve_retornar_todos_os_usuarios()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");

        await client.RegisterUser(CreateUserIn.New(faculdade.Id, Academico));
        await client.RegisterUser(CreateUserIn.New(faculdade.Id, Professor));
        await client.RegisterUser(CreateUserIn.New(faculdade.Id, Aluno));

        // Act
        var users = await client.GetAsync<List<CreateUserOut>>("/users");

        // Assert
        users.Count.Should().BeGreaterThanOrEqualTo(4);
    }
}
