using Syki.Shared;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Services;

public class AgendasService : IAgendasService
{
    private readonly SykiDbContext _ctx;
    public AgendasService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<List<AgendaDiaOut>> GetAluno(Guid faculdadeId, Guid userId)
    {
        var turmas = await _ctx.Turmas.AsNoTracking()
            .Include(t => t.Disciplina)
            .Include(t => t.Horarios)
            .Where(c => c.FaculdadeId == faculdadeId)
            .ToListAsync();

        var result = turmas.ConvertAll(t =>
        {
            return new AgendaDiaOut
            {
                Dia = t.Horarios[0].Dia,
                Disciplinas =
                [
                    new AgendaDisciplinaOut
                    {
                        Nome = t.Disciplina.Nome,
                        Start = t.Horarios[0].Start,
                        End = t.Horarios[0].End
                    }
                ]
            };
        });
        
        return result;
    }
}
