using Syki.Shared.GetBooks;
using Syki.Shared.CreateBook;

namespace Syki.Back.CreateBook;

public class Book
{
    public Guid Id { get; }
    public Guid InstitutionId { get; }
    public string Title { get; }

    private Book() { }

    public Book(Guid institutionId, string title)
    {
        Id = Guid.NewGuid();
        InstitutionId = institutionId;
        Title = title;
    }

    public CreateBookOut ToCreateBookOut()
    {
        return new CreateBookOut
        {
            Id = Id,
            Title = Title,
        };
    }

    public GetBooksOut ToGetBooksOut()
    {
        return new GetBooksOut
        {
            Id = Id,
            Title = Title,
        };
    }
}
