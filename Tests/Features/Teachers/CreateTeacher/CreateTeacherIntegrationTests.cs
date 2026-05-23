namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Teachers_CreateTeacher_Should_create_teacher()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        // Act
        var result = await client.CreateTeacher(DataGen.UserName, DataGen.Email);

        // Assert
        result.ShouldBeSuccess();
        var teacher = result.Success;
        teacher.Id.Should().BeGreaterThan(0);
    }
}
