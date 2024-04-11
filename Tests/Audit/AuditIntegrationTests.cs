using Audit.Core;

namespace Syki.Tests.Integration;

public class AuditIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Deve_auditar_a_criacao_de_um_campus()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        Configuration.AuditDisabled = false;
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");
        Configuration.AuditDisabled = true;

        // Assert
        using var ctx = _factory.GetDbContext();
        var audit = await ctx.AuditLogs.FirstAsync(a => a.EntityId == campus.Id);
        audit.Action.Should().Be("Insert");
    }

    [Test]
    public async Task Deve_auditar_a_atualizacao_de_um_campus()
    {
        // Arrange

        var client = await _factory.LoggedAsAcademico();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");

        // Act
        Configuration.AuditDisabled = false;
        await client.UpdateCampus(campus.Id, "Agreste II", "Bonito - PE");
        Configuration.AuditDisabled = true;

        // Assert
        using var ctx = _factory.GetDbContext();
        var audit = await ctx.AuditLogs.FirstAsync(a => a.EntityId == campus.Id);
        audit.Action.Should().Be("Update");
    }
}
