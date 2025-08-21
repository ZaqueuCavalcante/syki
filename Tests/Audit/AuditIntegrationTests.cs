using Syki.Back.Features.Academic.CreateCampus;

namespace Syki.Tests.Integration;

[Parallelizable(ParallelScope.All)]
public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_audit_campus_creation()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();

        // Act
        CampusOut campus = await client.CreateCampus();

        // Assert
        await using var ctx = _api.GetDbContext();
        var audit = await ctx.AuditLogs.FirstAsync(a => a.EntityId == campus.Id);
        audit.Action.Should().Be("Insert");
        audit.EntityType.Should().Be(nameof(Campus));
    }

    [Test]
    public async Task Should_audit_campus_update()
    {
        // Arrange
        var client = await _api.LoggedAsAcademic();
        CampusOut campus = await client.CreateCampus();

        // Act
        await client.UpdateCampus(campus.Id, "Agreste II", BrazilState.PE, "Bonito");

        // Assert
        await using var ctx = _api.GetDbContext();
        var audit = await ctx.AuditLogs
            .OrderByDescending(x => x.CreatedAt)
            .FirstAsync(a => a.EntityId == campus.Id);
        audit.Action.Should().Be("Update");
        audit.EntityType.Should().Be(nameof(Campus));
    }
}
