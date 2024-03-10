using Audit.Core;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public class AuditIntegrationTests : IntegrationTestBase
{
    // [Test]
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

    // [Test]
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

    // [Test]
    public async Task Nao_deve_auditar_o_login_com_mfa()
    {
        // Arrange
        var client = _factory.CreateClient();
        var institution = await client.CreateInstitution();
        var user = await client.RegisterAndLogin(institution.Id, Academico);

        var keyResponse = await client.GetAsync<GetMfaKeyOut>("/mfa/key");
        var token = keyResponse.Key.ToMfaToken();
        await client.PostAsync<SetupMfaOut>("/mfa/setup", new SetupMfaIn { Token = token });

        client.RemoveAuthToken();

        var data = new LoginIn { Email = user.Email, Password = user.Password };
        await client.PostHttpAsync("/login", data);

        var body = new LoginMfaIn { Code = Guid.NewGuid().ToHashCode().ToString()[..6] };

        // Act
        Configuration.AuditDisabled = false;
        await client.PostHttpAsync("/login/mfa", body);
        Configuration.AuditDisabled = true;

        // Assert
        using var ctx = _factory.GetDbContext();
        var audit = await ctx.AuditLogs.FirstOrDefaultAsync(a => a.EntityType != "Campus");
        audit.Should().BeNull();
    }
}
