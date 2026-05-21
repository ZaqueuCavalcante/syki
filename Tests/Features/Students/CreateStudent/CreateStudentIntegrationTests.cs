namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Students_CreateStudent_Should_create_student()
    {
        // Arrange
        var client = await _back.LoggedAsDirectot();

        // Act
        var result = await client.CreateStudent(DataGen.UserName, DataGen.Email);

        // Assert
        result.ShouldBeSuccess();
        var student = result.Success;
        student.Id.Should().BeGreaterThan(0);
    }
}
