using Audit.Core;
using Syki.Shared;
using Syki.Tests.Base;
using NUnit.Framework;
using FluentAssertions;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public class AuditIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_auditar_a_criacao_de_um_campus()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await client.RegisterUser(user);
        await client.Login(user.Email, user.Password);

        var body = new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" };

        // Act
        Configuration.AuditDisabled = false;
        var campus = await client.PostAsync<CampusOut>("/campi", body);
        Configuration.AuditDisabled = true;

        // Assert
        using var scope = _factory.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        var audit = await ctx.AuditLogs.FirstAsync(a => a.EntityId == campus.Id);

        audit.Action.Should().Be("Insert");
        audit.FaculdadeId.Should().Be(faculdade.Id);
    }

    [Test]
    public async Task Deve_auditar_a_atualizacao_de_um_campus()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await client.RegisterUser(user);
        await client.Login(user.Email, user.Password);

        var body = new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" };
        var campus = await client.PostAsync<CampusOut>("/campi", body);

        // Act
        campus.Nome = "Agreste II";
        campus.Cidade = "Bonito - PE";
        Configuration.AuditDisabled = false;
        await client.PutAsync<CampusOut>("/campi", campus);
        Configuration.AuditDisabled = true;

        // Assert
        using var scope = _factory.Services.CreateScope();
         var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        var audit = await ctx.AuditLogs.FirstAsync(a => a.EntityId == campus.Id);

        audit.Action.Should().Be("Update");
        audit.FaculdadeId.Should().Be(faculdade.Id);
    }

    [Test]
    public async Task Nao_deve_auditar_o_login_com_mfa()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await client.RegisterUser(user);
        await client.Login(user.Email, user.Password);

        var keyResponse = await client.GetAsync<MfaKeyOut>("/users/mfa-key");
        var token = keyResponse.Key.ToMfaToken();
        await client.PostAsync<MfaSetupOut>("/users/mfa-setup", new MfaSetupIn { Token = token });

        client.DefaultRequestHeaders.Remove("Authorization");

        var data = new LoginIn { Email = user.Email, Password = user.Password };
        await client.PostAsync("/users/login", data.ToStringContent());

        var body = new LoginMfaIn { Code = Guid.NewGuid().ToHashCode().ToString()[..6] };

        // Act
        Configuration.AuditDisabled = false;
        await client.PostAsync("/users/login-mfa", body.ToStringContent());
        Configuration.AuditDisabled = true;

        // Assert
        using var scope = _factory.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        var audit = await ctx.AuditLogs.FirstOrDefaultAsync(a => a.EntityType != "Campus");

        audit.Should().BeNull();
    }
}
