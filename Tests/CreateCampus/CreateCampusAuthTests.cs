namespace Syki.Tests.Auth;

public partial class AuthTests : AuthTestBase
{
    [Test]
    public async Task Should_return_unauthorized_when_unauthenticated_user_wants_create_a_campus()
    {
        // Arrange
        Logout();

        // Act
        var response = await _client.PostAsync("/campi", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    [TestCaseSource(typeof(TestData), nameof(TestData.AllRolesExceptAcademico))]
    public async Task Should_return_forbidden_when_user_has_no_permission_to_create_a_campus(string role)
    {
        // Arrange
        Login(role);

        // Act
        var response = await _client.PostAsync("/campi", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
