namespace Syki.Back.GetAlunoDisciplinas;

public class GetAlunoDisciplinasService(SykiDbContext ctx)
{
    public async Task<List<DisciplinaOut>> Get(Guid userId)
    {
        var ofertaId = await ctx.Alunos.Where(a => a.Id == userId)
            .Select(a => a.OfertaId).FirstAsync();
        var gradeId = await ctx.Ofertas.Where(o => o.Id == ofertaId)
            .Select(o => o.GradeId).FirstAsync();

        var grade = await ctx.Grades.AsNoTracking()
            .Include(g => g.Curso)
            .Include(g => g.Disciplinas)
            .Include(g => g.Vinculos)
            .FirstAsync(g => g.Id == gradeId);

        var response = grade.ToOut().Disciplinas.OrderBy(d => d.Periodo).ToList();

        var situacoes = await ctx.TurmaAlunos.AsNoTracking()
            .Where(x => x.AlunoId == userId)
            .ToListAsync();

        var ids = grade.Disciplinas.ConvertAll(d => d.Id);
        var turmas = await ctx.Turmas.Where(t => ids.Contains(t.DisciplinaId))
            .Select(x => new { x.Id, x.DisciplinaId }).ToListAsync();

        response.ForEach(r =>
        {
            var turmaId = turmas.FirstOrDefault(x => x.DisciplinaId == r.Id)?.Id;
            r.Situacao = situacoes.FirstOrDefault(x => x.TurmaId == turmaId)?.Situacao ?? Situacao.Pendente;
        });

        return response;
    }
}
