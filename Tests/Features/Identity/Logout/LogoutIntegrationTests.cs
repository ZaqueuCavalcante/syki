namespace Estud.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Identity_Logout_Should_logout()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var response = await client.Logout();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var testResponse = await client.CreateCampus();
        testResponse.ShouldBeError(HttpStatusCode.Unauthorized);
    }
}
