using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Extensions;
using Syki.Back.Exceptions;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

[TestFixture]
public class UsersIntegrationTests : IntegrationTestBase
{
    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.AllRolesExceptAdm))]
    public async Task Deve_registrar_um_novo_usuario_com_role_permitida(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var body = UserIn.New(faculdade.Id, role);

        // Act
        var user = await PostAsync<UserOut>("/users", body);

        // Assert
        user.Id.Should().NotBeEmpty();
        user.Nome.Should().Be(body.Name);
        user.Email.Should().Be(body.Email);
        user.FaculdadeId.Should().Be(body.Faculdade);
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.InvalidRoles))]
    public async Task Nao_deve_registrar_um_novo_usuario_com_role_invalida(string role)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var body = UserIn.New(faculdade.Id, role);

        // Act
        var response = await _client.PostAsync("/users", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0010);  
    }

    [Test]
    public async Task Nao_deve_registrar_um_novo_usuario_com_faculdade_invalida()
    {
        // Arrange
        await Login("adm@syki.com", "Adm@123");

        var body = UserIn.New(Guid.NewGuid(), Academico);

        // Act
        var response = await _client.PostAsync("/users", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0011);  
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.InvalidEmails))]
    public async Task Nao_deve_registrar_um_novo_usuario_com_email_invalido(string email)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var body = new UserIn
        {
            Faculdade = faculdade.Id,
            Name = "Acadêmico - Nova Roma",
            Email = email,
            Password = "Academico@123",
            Role = Academico,
        };

        // Act
        var response = await _client.PostAsync("/users", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0013);  
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.InvalidPasswords))]
    public async Task Nao_deve_registrar_um_novo_usuario_com_senha_fraca(string password)
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var body = new UserIn
        {
            Faculdade = faculdade.Id,
            Name = "Acadêmico - Nova Roma",
            Email = "academico@novaroma.com",
            Password = password,
            Role = Academico,
        };

        // Act
        var response = await _client.PostAsync("/users", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0012);  
    }

    [Test]
    public async Task Nao_deve_registrar_um_novo_usuario_com_email_duplicado()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var body = new UserIn
        {
            Faculdade = faculdade.Id,
            Name = "Acadêmico - Nova Roma",
            Email = "academico@novaroma.com",
            Password = "Academico@123",
            Role = Academico,
        };
        await _client.PostAsync("/users", body.ToStringContent());

        // Act
        var response = await _client.PostAsync("/users", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0014);  
    }

    [Test]
    public async Task Nao_deve_logar_usuario_que_nao_existe()
    {
        // Arrange
        var data = new LoginIn { Email = "academico@novaroma.com", Password = "Academico@123" };

        // Act
        var response = await _client.PostAsync("/users/login", data.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<LoginOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.WrongEmailOrPassword.Should().BeTrue();
    }

    [Test]
    public async Task Nao_deve_logar_usuario_com_email_errado()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);

        var data = new LoginIn { Email = user.Email + "lalala", Password = user.Password };

        // Act
        var response = await _client.PostAsync("/users/login", data.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<LoginOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.WrongEmailOrPassword.Should().BeTrue();
    }

    [Test]
    public async Task Nao_deve_logar_usuario_com_senha_errada()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);

        var data = new LoginIn { Email = user.Email, Password = user.Password + "lalala" };

        // Act
        var response = await _client.PostAsync("/users/login", data.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<LoginOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.WrongEmailOrPassword.Should().BeTrue();
    }

    [Test]
    public async Task Deve_logar_usuario_sem_mfa()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);

        var data = new LoginIn { Email = user.Email, Password = user.Password };

        // Act
        var response = await PostAsync<LoginOut>("/users/login", data);

        // Assert
        response.AccessToken.Should().StartWith("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.");
    }

    [Test]
    public async Task Nao_deve_logar_quando_informar_email_e_senha_corretos_mas_precisa_de_mfa()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var keyResponse = await GetAsync<MfaKeyOut>("/users/mfa-key");
        var token = keyResponse.Key.ToMfaToken();
        await PostAsync<MfaSetupOut>("/users/mfa-setup", new MfaSetupIn { Token = token });

        _client.DefaultRequestHeaders.Remove("Authorization");

        var data = new LoginIn { Email = user.Email, Password = user.Password };

        // Act
        var response = await _client.PostAsync("/users/login", data.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<LoginOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.RequiresTwoFactor.Should().BeTrue();
    }

    [Test]
    public async Task Nao_deve_logar_quando_informar_email_e_senha_corretos_mas_precisa_de_mfa_e_informa_codigo_errado()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var keyResponse = await GetAsync<MfaKeyOut>("/users/mfa-key");
        var token = keyResponse.Key.ToMfaToken();
        await PostAsync<MfaSetupOut>("/users/mfa-setup", new MfaSetupIn { Token = token });

        _client.DefaultRequestHeaders.Remove("Authorization");

        var data = new LoginIn { Email = user.Email, Password = user.Password };
        await _client.PostAsync("/users/login", data.ToStringContent());

        var body = new LoginMfaIn { Code = Guid.NewGuid().ToHashCode().ToString()[..6] };

        // Act
        var response = await _client.PostAsync("/users/login-mfa", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<LoginOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Wrong2FactorCode.Should().BeTrue();
    }

    [Test]
    public async Task Nao_deve_logar_quando_tentar_obter_o_jwt_sem_informar_email_e_senha_antes()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var keyResponse = await GetAsync<MfaKeyOut>("/users/mfa-key");
        var token = keyResponse.Key.ToMfaToken();
        await PostAsync<MfaSetupOut>("/users/mfa-setup", new MfaSetupIn { Token = token });

        _client.DefaultRequestHeaders.Remove("Authorization");

        var body = new LoginMfaIn { Code = token };

        // Act
        var response = await _client.PostAsync("/users/login-mfa", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<LoginOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Wrong2FactorCode.Should().BeTrue();
    }

    [Test]
    public async Task Deve_logar_quando_informar_email_e_senha_corretos_mas_precisa_de_mfa_e_informa_codigo_correto()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");
        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var keyResponse = await GetAsync<MfaKeyOut>("/users/mfa-key");
        var token = keyResponse.Key.ToMfaToken();
        await PostAsync<MfaSetupOut>("/users/mfa-setup", new MfaSetupIn { Token = token });

        _client.DefaultRequestHeaders.Remove("Authorization");

        var data = new LoginIn { Email = user.Email, Password = user.Password };
        await _client.PostAsync("/users/login", data.ToStringContent());

        var body = new LoginMfaIn { Code = token };

        // Act
        var response = await PostAsync<LoginOut>("/users/login-mfa", body);

        // Assert
        response.AccessToken.Should().StartWith("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.");
    }

    [Test]
    public async Task Deve_retornar_todos_os_usuarios()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        await RegisterUser(UserIn.New(faculdade.Id, Academico));
        await RegisterUser(UserIn.New(faculdade.Id, Professor));
        await RegisterUser(UserIn.New(faculdade.Id, Aluno));

        // Act
        var users = await GetAsync<List<UserOut>>("/users");

        // Assert
        users.Count.Should().Be(4);
    }
}
