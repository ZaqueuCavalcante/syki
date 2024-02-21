using Syki.Back.Database;
using Syki.Shared.CreateBook;

namespace Syki.Back.CreateBook;

public class CreateBookService
{
    private readonly SykiDbContext _ctx;
    public CreateBookService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<CreateBookOut> Create(Guid institutionId, CreateBookIn data)
    {
        var book = new Book(institutionId, data.Title);

        _ctx.Add(book);
        await _ctx.SaveChangesAsync();

        return book.ToCreateBookOut();
    }
}
