using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Tests.Integration;

public class ExceptionsIntegrationTests : IntegrationTestBase
{
    // [Test]
    public async Task Deve_lidar_com_um_internal_server_error()
    {
        // Arrange
        var client = _factory.CreateClient();
        using var ctx = _factory.GetDbContext();

        var academicoRole = await ctx.Roles.FirstAsync(r => r.Name == "Academico");
        ctx.Remove(academicoRole);
        await ctx.SaveChangesAsync();

        var faculdade = await client.CreateInstitution("Nova Roma");
        var user = CreateUserIn.New(faculdade.Id, Academico);

        // Act
        var response = await client.PostAsync("/users", user.ToStringContent());

        // Assert
        var error = await response.DeserializeTo<ErrorOut>();
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        error.Message.Should().Be("Internal Server Error");
    }
}
