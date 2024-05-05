namespace Syki.Back.GetProfessorAgenda;

public class GetProfessorAgendaService(SykiDbContext ctx)
{
    public async Task<List<AgendaDayOut>> Get(Guid institution, Guid userId)
    {
        var turmas = await ctx.Turmas.AsNoTracking()
            .Include(t => t.Disciplina)
            .Include(t => t.Horarios)
            .Where(t => t.InstitutionId == institution && t.ProfessorId == userId)
            .ToListAsync();

        var response = turmas.ConvertAll(t =>
        {
            return new MatriculaTurmaOut
            {
                Disciplina = t.Disciplina.Name,
                Horarios = t.Horarios.ConvertAll(h => h.ToOut()),
            };
        });
        
        return response.ToAgendas();
    }
}
