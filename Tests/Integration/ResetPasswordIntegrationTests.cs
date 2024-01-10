using System.Net;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Services;
using Syki.Back.Database;
using Syki.Back.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_retornar_o_token_de_reset_de_senha()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");

        var user = await client.RegisterUser(UserIn.New(faculdade.Id, Academico));

        using var scope = _factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IAuthService>();

        // Act
        var response = await service.GetResetPasswordToken(user.Id);

        // Assert
        response.Token.Length.Should().Be(240);
    }

    [Test]
    public async Task Deve_salvar_o_reset_password_ao_buscar_o_token_de_reset_de_senha()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");

        var user = await client.RegisterUser(UserIn.New(faculdade.Id, Academico));

        using var scope = _factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IAuthService>();
        var response = await service.GetResetPasswordToken(user.Id);

        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        // Act
        var reset = await ctx.ResetPasswords.FirstAsync(r => r.UserId == user.Id);

        // Assert
        reset.Token.Should().Be(response.Token);
    }

    [Test]
    public async Task Nao_deve_retornar_o_token_de_reset_de_senha_quando_o_usuario_nao_existir()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IAuthService>();

        // Act
        Func<Task> act = async () => await service.GetResetPasswordToken(Guid.NewGuid());

        // Assert
        await act.Should().ThrowAsync<DomainException>().WithMessage(ExceptionMessages.DE0016);
    }

    [Test]
    public async Task Nao_deve_resetar_a_senha_quando_o_token_estiver_errado()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");

        var user = await client.RegisterUser(UserIn.New(faculdade.Id, Academico));

        using var scope = _factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IAuthService>();
        await service.GetResetPasswordToken(user.Id);

        var body = new ResetPasswordIn { Token = Guid.NewGuid().ToString(), Password = "My@newP4ssword" };

        // Act
        var response = await client.PostAsync("/users/reset-password", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0016); 
    }

    [Test]
    public async Task Deve_resetar_a_senha_do_usuario()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");

        var user = await client.RegisterUser(UserIn.New(faculdade.Id, Academico));

        using var scope = _factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IAuthService>();
        var tokenResponse = await service.GetResetPasswordToken(user.Id);

        var body = new ResetPasswordIn { Token = tokenResponse.Token, Password = "My@newP4ssword" };

        // Act
        var response = await client.PostAsync<ResetPasswordOut>("/users/reset-password", body);

        // Assert
        response.Ok.Should().BeTrue();
    }

    [Test]
    public async Task Deve_fazer_login_com_a_nova_senha_apos_resetar_a_senha()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");

        var user = await client.RegisterUser(UserIn.New(faculdade.Id, Academico));

        using var scope = _factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IAuthService>();
        var tokenResponse = await service.GetResetPasswordToken(user.Id);

        var body = new ResetPasswordIn { Token = tokenResponse.Token, Password = "My@newP4ssword" };
        await client.PostAsync<ResetPasswordOut>("/users/reset-password", body);

        var data = new LoginIn { Email = user.Email, Password = body.Password };

        // Act
        var response = await client.PostAsync<LoginOut>("/users/login", data);

        // Assert
        response.AccessToken.Should().StartWith("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.");
    }

    [Test]
    public async Task Nao_deve_fazer_login_com_a_senha_antiga_apos_resetar_a_senha()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");

        var userIn = UserIn.New(faculdade.Id, Academico);
        var userOut = await client.RegisterUser(userIn);

        using var scope = _factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IAuthService>();
        var tokenResponse = await service.GetResetPasswordToken(userOut.Id);

        var body = new ResetPasswordIn { Token = tokenResponse.Token, Password = "My@newP4ssword" };
        await client.PostAsync<ResetPasswordOut>("/users/reset-password", body);

        var data = new LoginIn { Email = userIn.Email, Password = userIn.Password };

        // Act
        var response = await client.PostAsync<LoginOut>("/users/login", data);

        // Assert
        response.WrongEmailOrPassword.Should().BeTrue();
    }

    [Test]
    [TestCaseSource(typeof(TestDataStreams), nameof(TestDataStreams.InvalidPasswords))]
    public async Task Nao_deve_resetar_a_senha_do_usuario_quando_a_senha_for_fraca(string password)
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");

        var user = await client.RegisterUser(UserIn.New(faculdade.Id, Academico));

        using var scope = _factory.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IAuthService>();
        var tokenResponse = await service.GetResetPasswordToken(user.Id);

        var body = new ResetPasswordIn { Token = tokenResponse.Token, Password = password };

        // Act
        var response = await client.PostAsync("/users/reset-password", body.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        error.Message.Should().Be(ExceptionMessages.DE0012); 
    }
}
