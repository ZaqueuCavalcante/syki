using Syki.Back.CreateBook;

namespace Syki.Tests.CreateBook;

public class CreateBookUnitTests
{
    [Test]
    public void Should_create_a_book_with_not_empty_id()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string title = "UFPE";

        // Act
        var book = new Book(institutionId, title);

        // Assert
        book.Id.Should().NotBeEmpty();
    }

    [Test]
    public void Should_create_a_book_with_right_institution_id()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string title = "UFPE";

        // Act
        var book = new Book(institutionId, title);

        // Assert
        book.InstitutionId.Should().Be(institutionId);
    }

    [Test]
    public void Should_create_a_book_with_right_title()
    {
        // Arrange
        var institutionId = Guid.NewGuid();
        const string title = "UFPE";

        // Act
        var book = new Book(institutionId, title);

        // Assert
        book.Title.Should().Be(title);
    }

    [Test]
    public void Should_convert_book_to_create_book_out()
    {
        // Arrange
        var book = new Book(Guid.NewGuid(), "UFPE");

        // Act
        var bookOut = book.ToCreateBookOut();

        // Assert
        bookOut.Id.Should().Be(book.Id);
        bookOut.Title.Should().Be(book.Title);
    }
}
