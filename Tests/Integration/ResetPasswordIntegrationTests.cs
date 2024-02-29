using Syki.Shared;
using Syki.Shared.Login;
using Syki.Shared.CreateUser;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_retornar_o_token_de_reset_de_senha()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        var user = await client.RegisterUser(CreateUserIn.New(faculdade.Id, Academico));

        // Act
        var token = await _factory.GetResetPasswordToken(user.Id);

        // Assert
        token!.Length.Should().Be(36);
    }

    [Test]
    public async Task Deve_salvar_o_reset_password_ao_criar_um_usuario()
    {
        // Arrange
        var client = _factory.GetClient();
        var faculdade = await client.CreateInstitution();
        var user = await client.RegisterUser(CreateUserIn.New(faculdade.Id, Academico));

        var token = await _factory.GetResetPasswordToken(user.Id);
        using var ctx = _factory.GetDbContext();

        // Act
        var reset = await ctx.ResetPasswordTokens.FirstAsync(r => r.UserId == user.Id);

        // Assert
        reset.Id.Should().Be(token);
    }

    [Test]
    public async Task Nao_deve_retornar_o_token_de_reset_de_senha_quando_o_usuario_nao_existir()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var token = await _factory.GetResetPasswordToken(Guid.NewGuid());

        // Assert
        token.Should().BeNull();
    }

    [Test]
    public async Task Nao_deve_resetar_a_senha_quando_o_token_estiver_errado()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        await client.RegisterUser(CreateUserIn.New(faculdade.Id, Academico));

        client.RemoveAuthToken();
        var body = new ResetPasswordIn { Token = Guid.NewGuid().ToString(), Password = "My@new@strong@P4ssword" };

        // Act
        var response = await client.PostHttpAsync("/reset-password", body);

        // Assert
        await response.AssertBadRequest(Throw.DE019);
    }

    [Test]
    public async Task Deve_resetar_a_senha_do_usuario()
    {
        // Arrange
        var client = _factory.GetClient();
        var faculdade = await client.CreateInstitution();
        var user = await client.RegisterUser(CreateUserIn.New(faculdade.Id, Academico));

        client.RemoveAuthToken();
        var token = await _factory.GetResetPasswordToken(user.Id);
        var body = new ResetPasswordIn { Token = token!, Password = "My@newP4sswordMy@newP4ssword" };

        // Act
        var response = await client.PostAsync("/reset-password", body.ToStringContent());

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Test]
    public async Task Deve_fazer_login_com_a_nova_senha_apos_resetar_a_senha()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");

        var user = await client.RegisterUser(CreateUserIn.New(faculdade.Id, Academico));

        client.RemoveAuthToken();
        var token = await _factory.GetResetPasswordToken(user.Id);

        var body = new ResetPasswordIn { Token = token!, Password = "My@new@strong@P4ssword" };
        await client.PostAsync("/reset-password", body);

        var data = new LoginIn { Email = user.Email, Password = body.Password };

        // Act
        var response = await client.PostAsync<LoginOut>("/login", data);

        // Assert
        response.AccessToken.Should().StartWith("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.");
    }

    [Test]
    public async Task Nao_deve_fazer_login_com_a_senha_antiga_apos_resetar_a_senha()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        var userIn = CreateUserIn.New(faculdade.Id, Academico);
        var userOut = await client.RegisterUser(userIn);

        client.RemoveAuthToken();
        var token = await _factory.GetResetPasswordToken(userOut.Id);

        var body = new ResetPasswordIn { Token = token!, Password = "My@new@strong@P4ssword" };
        await client.PostAsync("/reset-password", body);

        var data = new LoginIn { Email = userIn.Email, Password = userIn.Password };

        // Act
        var response = await client.PostAsync<LoginOut>("/login", data);

        // Assert
        response.WrongEmailOrPassword.Should().BeTrue();
    }

    [Test]
    public async Task Nao_deve_resetar_a_senha_do_usuario_pela_segunda_vez_usando_o_mesmo_token()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        var user = await client.RegisterUser(CreateUserIn.New(faculdade.Id, Academico));

        client.RemoveAuthToken();
        var token = await _factory.GetResetPasswordToken(user.Id);

        var body = new ResetPasswordIn { Token = token!, Password = "My@new@strong@P4ssword" };
        await client.PostHttpAsync("/reset-password", body);

        // Act
        var response = await client.PostHttpAsync("/reset-password", body);

        // Assert
        await response.AssertBadRequest(Throw.DE020);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.InvalidPasswords))]
    public async Task Nao_deve_resetar_a_senha_do_usuario_quando_a_senha_for_fraca(string password)
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        var user = await client.RegisterUser(CreateUserIn.New(faculdade.Id, Academico));

        client.RemoveAuthToken();
        var token = await _factory.GetResetPasswordToken(user.Id);

        var body = new ResetPasswordIn { Token = token!, Password = password };

        // Act
        var response = await client.PostHttpAsync("/reset-password", body);

        // Assert
        await response.AssertBadRequest(Throw.DE015);
    }
}
