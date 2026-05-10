namespace Syki.Tests.Extensions;

public class EnvExtensionsUnitTests
{
    // [Test]
    public void Should_get_deploy_hash()
    {
        // Arrange / Act
        var hash = EnvironmentExtensions.DeployHash;

        // Assert
        hash.Should().HaveLength(8);
    }

    // [Test]
    public void Should_get_env_as_testing()
    {
        // Arrange
        EnvironmentExtensions.SetAsTesting();

        // Act
        var env = EnvironmentExtensions.IsTesting();

        // Assert
        env.Should().BeTrue();
    }
}
