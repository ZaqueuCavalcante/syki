using Syki.Back.Database;
using Syki.Shared.GetBooks;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.GetBooks;

public class GetBooksService
{
    private readonly SykiDbContext _ctx;
    public GetBooksService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<List<GetBooksOut>> Get(Guid institutionId)
    {
        var books = await _ctx.Books
            .Where(c => c.InstitutionId == institutionId)
            .ToListAsync();
        
        return books.ConvertAll(p => p.ToGetBooksOut());
    }
}
