namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_create_a_book()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        var book = await client.CreateBook("Manual de DevOps");

        // Assert
        book.Id.Should().NotBeEmpty();
        book.Title.Should().Be("Manual de DevOps");
    }
}
