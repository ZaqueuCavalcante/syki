using Syki.Shared;
using Syki.Back.Domain;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Services;

public class ProfessoresService : IProfessoresService
{
    private readonly SykiDbContext _ctx;
    public ProfessoresService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<ProfessorOut> Create(Guid faculdadeId, ProfessorIn data)
    {
        var professor = new Professor(faculdadeId, data.Nome);

        await _ctx.Professores.AddAsync(professor);

        await _ctx.SaveChangesAsync();

        return professor.ToOut();
    }

    public async Task<List<ProfessorOut>> GetAll(Guid faculdadeId)
    {
        var professores = await _ctx.Professores
            .Where(c => c.FaculdadeId == faculdadeId)
            .ToListAsync();

        return professores.ConvertAll(p => p.ToOut());
    }
}
