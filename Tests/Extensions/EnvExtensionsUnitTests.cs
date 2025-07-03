namespace Syki.Tests.Extensions;

public class EnvExtensionsUnitTests
{
    [Test]
    public void Should_get_last_commit_hash()
    {
        // Arrange / Act
        var hash = Env.GetLastCommitHash();

        // Assert
        hash.Should().HaveLength(16);
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
