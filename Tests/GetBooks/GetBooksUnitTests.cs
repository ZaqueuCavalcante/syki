using NUnit.Framework;
using FluentAssertions;
using Syki.Back.CreateBook;

namespace Syki.Tests.CreateBook;

public class GetBooksUnitTests
{
    [Test]
    public void Should_convert_book_to_get_books_out()
    {
        // Arrange
        var book = new Book(Guid.NewGuid(), "UFPE");

        // Act
        var bookOut = book.ToGetBooksOut();

        // Assert
        bookOut.Id.Should().Be(book.Id);
        bookOut.Title.Should().Be(book.Title);
    }
}
