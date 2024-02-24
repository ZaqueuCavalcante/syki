using Syki.Shared.CreateBook;

namespace Syki.Tests.Integration;

public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_get_empty_list_when_has_no_books()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        // Act
        var books = await client.GetAsync<List<BookOut>>("/books");

        // Assert
        books.Should().BeEmpty();
    }

    [Test]
    public async Task Should_get_many_books()
    {
        // Arrange
        var client = await _factory.LoggedAsAcademico();

        await client.CreateBook("Manual de DevOps");
        await client.CreateBook("O Projeto do Projeto");

        // Act
        var books = await client.GetAsync<List<BookOut>>("/books");

        // Assert
        books.Should().HaveCount(2);
    }

    [Test]
    public async Task Should_get_only_institution_books()
    {
        // Arrange
        var client = _factory.CreateClient();
        var userNovaRoma = await client.NewAcademico("Nova Roma");
        var userUfpe = await client.NewAcademico("UFPE");

        await client.Login(userNovaRoma);
        await client.CreateBook("Manual de DevOps");

        await client.Login(userUfpe);
        await client.CreateBook("O Projeto do Projeto");

        // Act
        await client.Login(userNovaRoma);

        // Assert
        var books = await client.GetAsync<List<BookOut>>("/books");
        books.Should().HaveCount(1);
        books[0].Title.Should().Be("Manual de DevOps");
    }
}
