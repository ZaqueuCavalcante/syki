namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    #region Happy path

    [Test]
    public async Task Identity_CheckSocialLoginAvailability_Should_return_available_social_login_providers()
    {
        // Arrange
        var client = _back.GetTestsClient();

        // Act
        var result = await client.CheckSocialLoginAvailability();

        // Assert
        var availability = result.Success;
        availability.GoogleEnabled.Should().BeTrue();
        availability.GoogleClientId.Should().Be("test-google-client-id");
    }

    #endregion
}
