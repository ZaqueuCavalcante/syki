namespace Syki.Back.GetProfessorAgenda;

public class GetProfessorAgendaService(SykiDbContext ctx)
{
    public async Task<List<AgendaDiaOut>> Get(Guid institution, Guid userId)
    {
        var turmas = await ctx.Turmas.AsNoTracking()
            .Include(t => t.Disciplina)
            .Include(t => t.Horarios)
            .Where(t => t.FaculdadeId == institution && t.ProfessorId == userId)
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
