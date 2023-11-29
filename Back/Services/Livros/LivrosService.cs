using Syki.Shared;
using Syki.Back.Domain;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Services;

public class LivrosService : ILivrosService
{
    private readonly SykiDbContext _ctx;
    public LivrosService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<LivroOut> Create(Guid faculdadeId, LivroIn data)
    {
        var livro = new Livro(faculdadeId, data.Titulo);

        await _ctx.Livros.AddAsync(livro);
        await _ctx.SaveChangesAsync();

        return livro.ToOut();
    }

    public async Task<List<LivroOut>> GetAll(Guid faculdadeId)
    {
        var livros = await _ctx.Livros
            .Where(c => c.FaculdadeId == faculdadeId)
            .ToListAsync();
        
        return livros.ConvertAll(p => p.ToOut());
    }
}
