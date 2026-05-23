namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Students_GetStudents_Should_get_students()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        await client.CreateStudent("Carlos Souza", DataGen.Email);
        await client.CreateStudent("Ana Lima", DataGen.Email);

        // Act
        var result = await client.GetStudents();

        // Assert
        result.Total.Should().Be(2);
        result.Items.First().Name.Should().Be("Ana Lima");
        result.Items.Last().Name.Should().Be("Carlos Souza");
    }
}
