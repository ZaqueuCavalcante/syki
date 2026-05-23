namespace Syki.Tests.Integration;

public partial class IntegrationTests
{
    [Test]
    public async Task Disciplines_GetDisciplines_Should_get_disciplines()
    {
        // Arrange
        var client = await _back.LoggedAsDirector();

        await client.CreateDiscipline("Química I");
        await client.CreateDiscipline("Física I");

        // Act
        var result = await client.GetDisciplines();

        // Assert
        result.Total.Should().Be(2);
        result.Items.First().Name.Should().Be("Física I");
        result.Items.Last().Name.Should().Be("Química I");
    }
}
