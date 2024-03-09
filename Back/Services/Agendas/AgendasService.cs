namespace Syki.Back.Services;

public class AgendasService : IAgendasService
{
    private readonly SykiDbContext _ctx;
    public AgendasService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<List<AgendaDiaOut>> GetAluno(Guid faculdadeId, Guid userId)
    {
        var ids = await _ctx.TurmaAlunos.AsNoTracking()
            .Where(x => x.AlunoId == userId && x.Situacao == Situacao.Matriculado)
            .Select(x => x.TurmaId)
            .ToListAsync();

        var turmas = await _ctx.Turmas.AsNoTracking()
            .Include(t => t.Disciplina)
            .Include(t => t.Horarios)
            .Where(t => t.FaculdadeId == faculdadeId && ids.Contains(t.Id))
            .ToListAsync();

        var response = turmas.ConvertAll(t =>
        {
            return new MatriculaTurmaOut
            {
                Disciplina = t.Disciplina.Nome,
                Horarios = t.Horarios.ConvertAll(h => h.ToOut()),
            };
        });
        
        return response.ToAgendas();
    }
}
