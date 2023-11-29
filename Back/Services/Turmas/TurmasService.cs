using Syki.Shared;
using Syki.Back.Domain;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Services;

public class TurmasService : ITurmasService
{
    private readonly SykiDbContext _ctx;
    public TurmasService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<TurmaOut> Create(Guid faculdadeId, TurmaIn data)
    {
        var turma = new Turma
        {
            Id = Guid.NewGuid(),
            FaculdadeId = faculdadeId,
            DisciplinaId = data.DisciplinaId,
            ProfessorId = data.ProfessorId,
            Periodo = data.Periodo,
        };
        _ctx.Turmas.Add(turma);

        data.Ofertas.ForEach(x =>
        {
            var vinculo = new OfertaTurma
            {
                OfertaId = x, TurmaId = turma.Id,
            };
            _ctx.Add(vinculo);
        });

        await _ctx.SaveChangesAsync();

        turma = await _ctx.Turmas
            .Include(t => t.Disciplina)
            .Include(t => t.Professor)
            .FirstAsync(x => x.Id == turma.Id);

        return turma.ToOut();
    }

    public async Task<List<TurmaOut>> GetAll(Guid faculdadeId)
    {
        var turmas = await _ctx.Turmas
            .Include(t => t.Disciplina)
            .Include(t => t.Professor)
            .Where(c => c.FaculdadeId == faculdadeId)
            .ToListAsync();

        return turmas.ConvertAll(t => t.ToOut());
    }
}
