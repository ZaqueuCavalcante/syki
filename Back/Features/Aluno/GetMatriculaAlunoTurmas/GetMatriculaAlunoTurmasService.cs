namespace Syki.Back.GetMatriculaAlunoTurmas;

public class GetMatriculaAlunoTurmasService(SykiDbContext ctx)
{
    public async Task<List<MatriculaTurmaOut>> Get(Guid institutionId, Guid userId)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var periodoDeMatricula = await ctx.EnrollmentPeriods.AsNoTracking()
            .Where(p => p.InstitutionId == institutionId && p.Start <= today && p.End >= today)
            .FirstOrDefaultAsync();
        
        if (periodoDeMatricula == null)
            return [];

        var ofertaId = await ctx.Alunos.Where(a => a.Id == userId)
            .Select(a => a.OfertaId).FirstAsync();
        var gradeId = await ctx.Ofertas.Where(o => o.Id == ofertaId)
            .Select(o => o.GradeId).FirstAsync();
        var grade = await ctx.Grades.Where(g => g.Id == gradeId).AsNoTracking()
            .Include(g => g.Vinculos)
            .FirstAsync();
        var ids = grade.Vinculos.ConvertAll(d => d.DisciplinaId);

        var turmas = await ctx.Turmas.AsNoTracking()
            .Include(t => t.Disciplina)
            .Include(t => t.Horarios)
            .Include(t => t.Professor)
            .Where(t => t.FaculdadeId == institutionId && t.Periodo == periodoDeMatricula.Id && ids.Contains(t.DisciplinaId))
            .ToListAsync();

        var selecteds = await ctx.TurmaAlunos.Where(x => x.AlunoId == userId).Select(x => x.TurmaId).ToListAsync();

        var response = turmas.ConvertAll(t =>
        {
            var vinculo = grade.Vinculos.First(v => v.DisciplinaId == t.DisciplinaId);
            return new MatriculaTurmaOut
            {
                Id = t.Id,
                Disciplina = t.Disciplina.Nome,
                Periodo = vinculo.Periodo,
                Creditos = vinculo.Creditos,
                CargaHoraria = vinculo.CargaHoraria,
                Professor = t.Professor.Nome,
                Horarios = t.Horarios.ConvertAll(h => h.ToOut()),
                IsSelected = selecteds.Contains(t.Id),
            };
        });

        return response.OrderBy(t => t.Periodo).ToList();
    }
}
