using Syki.Back.Database;
using Syki.Shared.CreateBook;

namespace Syki.Back.CreateBook;

public class CreateBookService(SykiDbContext ctx)
{
    public async Task<BookOut> Create(Guid institutionId, CreateBookIn data)
    {
        var book = new Book(institutionId, data.Title);

        ctx.Add(book);
        await ctx.SaveChangesAsync();

        return book.ToOut();
    }
}
