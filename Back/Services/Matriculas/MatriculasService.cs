using Syki.Shared;
using Syki.Back.Domain;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Services;

public class MatriculasService : IMatriculasService
{
    private readonly SykiDbContext _ctx;
    public MatriculasService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<PeriodoDeMatriculaOut> CreatePeriodoDeMatricula(Guid faculdadeId, PeriodoDeMatriculaIn data)
    {
        var periodo = new PeriodoDeMatricula(data.Id, faculdadeId, data.Start, data.End);

        _ctx.Add(periodo);
        await _ctx.SaveChangesAsync();

        return periodo.ToOut();
    }

    public async Task<List<PeriodoDeMatriculaOut>> GetPeriodosDeMatricula(Guid faculdadeId)
    {
        var periodos = await _ctx.PeriodosDeMatricula
            .Where(c => c.FaculdadeId == faculdadeId)
            .ToListAsync();

        return periodos.ConvertAll(p => p.ToOut());
    }

    public async Task<PeriodoDeMatriculaOut> GetPeriodoDeMatriculaAtual(Guid faculdadeId)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var periodoDeMatricula = await _ctx.PeriodosDeMatricula.AsNoTracking()
            .Where(p => p.FaculdadeId == faculdadeId && p.Start <= today && p.End >= today)
            .FirstOrDefaultAsync();
        
        if (periodoDeMatricula == null)
            return new PeriodoDeMatriculaOut { };
        
        return periodoDeMatricula.ToOut();
    }

    public async Task Create(Guid faculdadeId, Guid userId, MatriculaTurmaIn data)
    {
        var ids = await _ctx.Turmas
            .Where(t => t.FaculdadeId == faculdadeId && data.Turmas.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync();

        foreach (var id in ids)
        {
            _ctx.Add(new TurmaAluno(id, userId, Situacao.Matriculado));
        }

        await _ctx.SaveChangesAsync();
    }

    public async Task<List<MatriculaTurmaOut>> GetTurmas(Guid faculdadeId, Guid userId)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var periodoDeMatricula = await _ctx.PeriodosDeMatricula.AsNoTracking()
            .Where(p => p.FaculdadeId == faculdadeId && p.Start <= today && p.End >= today)
            .FirstOrDefaultAsync();
        
        if (periodoDeMatricula == null)
            return [];

        var ofertaId = await _ctx.Alunos.Where(a => a.Id == userId)
            .Select(a => a.OfertaId).FirstAsync();
        var gradeId = await _ctx.Ofertas.Where(o => o.Id == ofertaId)
            .Select(o => o.GradeId).FirstAsync();
        var grade = await _ctx.Grades.Where(g => g.Id == gradeId).AsNoTracking()
            .Include(g => g.Vinculos)
            .FirstAsync();
        var ids = grade.Vinculos.ConvertAll(d => d.DisciplinaId);

        var turmas = await _ctx.Turmas.AsNoTracking()
            .Include(t => t.Disciplina)
            .Include(t => t.Horarios)
            .Include(t => t.Professor)
            .Where(t => t.FaculdadeId == faculdadeId && t.Periodo == periodoDeMatricula.Id && ids.Contains(t.DisciplinaId))
            .ToListAsync();

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
            };
        });

        return response.OrderBy(t => t.Periodo).ToList();
    }
}
