using Syki.Back.Database;
using Syki.Shared.GetBooks;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.GetBooks;

public class GetBooksService(SykiDbContext ctx)
{
    public async Task<List<GetBooksOut>> Get(Guid institutionId)
    {
        var books = await ctx.Books.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId)
            .ToListAsync();
        
        return books.ConvertAll(p => p.ToGetBooksOut());
    }
}
