namespace Syki.Tests.Extensions;

public class EnvExtensionsUnitTests
{
    [Test]
    public void Should_get_deploy_hash()
    {
        // Arrange / Act
        var hash = Env.DeployHash;

        // Assert
        hash.Should().HaveLength(8);
    }

    [Test]
    public void Should_get_env_as_testing()
    {
        // Arrange
        Env.SetAsTesting();

        // Act
        var env = Env.IsTesting();

        // Assert
        env.Should().BeTrue();
    }
}
