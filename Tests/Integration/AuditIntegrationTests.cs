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

[TestFixture]
public class AuditIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_auditar_a_criacao_de_um_campus()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var body = new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" };

        // Act
        Configuration.AuditDisabled = false;
        var campus = await PostAsync<CampusOut>("/campi", body);
        Configuration.AuditDisabled = true;

        // Assert
        using var scope = _factory.Services.CreateScope();
        _ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        var audit = await _ctx.AuditLogs.FirstAsync();

        audit.EntityId.Should().Be(campus.Id);
        audit.Action.Should().Be("Insert");
        audit.FaculdadeId.Should().Be(faculdade.Id);
    }

    [Test]
    public async Task Deve_auditar_a_atualizacao_de_um_campus()
    {
        // Arrange
        var faculdade = await CreateFaculdade("Nova Roma");

        var user = UserIn.New(faculdade.Id, Academico);
        await RegisterUser(user);
        await Login(user.Email, user.Password);

        var body = new CampusIn { Nome = "Agreste I", Cidade = "Caruaru - PE" };
        var campus = await PostAsync<CampusOut>("/campi", body);

        // Act
        campus.Nome = "Agreste II";
        campus.Cidade = "Bonito - PE";
        Configuration.AuditDisabled = false;
        await PutAsync<CampusOut>("/campi", campus);
        Configuration.AuditDisabled = true;

        // Assert
        using var scope = _factory.Services.CreateScope();
        _ctx = scope.ServiceProvider.GetRequiredService<SykiDbContext>();

        var audit = await _ctx.AuditLogs.FirstAsync();

        audit.EntityId.Should().Be(campus.Id);
        audit.Action.Should().Be("Update");
        audit.FaculdadeId.Should().Be(faculdade.Id);
    }
}
