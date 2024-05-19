namespace Syki.Tests.Integration;

[Parallelizable(ParallelScope.All)]
public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_audit_campus_creation()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();

        // Act
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");

        // Assert
        using var ctx = _factory.GetDbContext();
        var audit = await ctx.AuditLogs.FirstAsync(a => a.EntityId == campus.Id);
        audit.Action.Should().Be("Insert");
    }

    [Test]
    public async Task Should_audit_campus_update()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademic();
        var campus = await client.CreateCampus("Agreste I", "Caruaru - PE");

        // Act
        await client.UpdateCampus(campus.Id, "Agreste II", "Bonito - PE");

        // Assert
        using var ctx = _factory.GetDbContext();
        var audit = await ctx.AuditLogs
            .OrderByDescending(x => x.CreatedAt)
            .FirstAsync(a => a.EntityId == campus.Id);
        audit.Action.Should().Be("Update");
    }
}
