namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Teachers_GetTeachers_Should_get_teachers()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        await client.CreateTeacher("Carlos Souza", DataGen.Email);
        await client.CreateTeacher("Ana Lima", DataGen.Email);

        // Act
        var result = await client.GetTeachers();

        // Assert
        result.Total.Should().Be(2);
        result.Items.First().Name.Should().Be("Ana Lima");
        result.Items.Last().Name.Should().Be("Carlos Souza");
    }
}
