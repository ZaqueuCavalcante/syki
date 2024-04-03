using Audit.Core;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public class AuditIntegrationTests : IntegrationTestBase
{
    [Test, Ignore("")]
    public async Task Deve_auditar_a_criacao_de_um_campus()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution();
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var body = new CreateCampusIn { Name = "Agreste I", City = "Caruaru - PE" };

        // Act
        Configuration.AuditDisabled = false;
        var campus = await client.PostAsync<CampusOut>("/campi", body);
        Configuration.AuditDisabled = true;

        // Assert
        using var ctx = _factory.GetDbContext();
        var audit = await ctx.AuditLogs.FirstAsync(a => a.EntityId == campus.Id);
        audit.Action.Should().Be("Insert");
        audit.FaculdadeId.Should().Be(faculdade.Id);
    }

    [Test, Ignore("")]
    public async Task Deve_auditar_a_atualizacao_de_um_campus()
    {
        // Arrange
        var client = _factory.CreateClient();
        var faculdade = await client.CreateInstitution("Nova Roma");
        await client.RegisterAndLogin(faculdade.Id, Academico);

        var body = new CreateCampusIn { Name = "Agreste I", City = "Caruaru - PE" };
        var campus = await client.PostAsync<CampusOut>("/campi", body);

        // Act
        campus.Name = "Agreste II";
        campus.City = "Bonito - PE";
        Configuration.AuditDisabled = false;
        await client.PutAsync<CampusOut>("/campi", campus);
        Configuration.AuditDisabled = true;

        // Assert
        using var ctx = _factory.GetDbContext();
        var audit = await ctx.AuditLogs.FirstAsync(a => a.EntityId == campus.Id);
        audit.Action.Should().Be("Update");
        audit.FaculdadeId.Should().Be(faculdade.Id);
    }
}
