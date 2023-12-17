using Syki.Shared;
using Syki.Back.Domain;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Services;

public class AlunosService : IAlunosService
{
    private readonly SykiDbContext _ctx;
    public AlunosService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<AlunoOut> Create(Guid faculdadeId, AlunoIn data)
    {
        var aluno = new Aluno(faculdadeId, data.Nome, data.OfertaId);

        await _ctx.Alunos.AddAsync(aluno);
        await _ctx.SaveChangesAsync();

        return aluno.ToOut();
    }

    public async Task<List<DisciplinaOut>> GetDisciplinas(Guid userId)
    {
        var ofertaId = await _ctx.Alunos.Where(a => a.UserId == userId)
            .Select(a => a.OfertaId).FirstAsync();
        var gradeId = await _ctx.Ofertas.Where(o => o.Id == ofertaId)
            .Select(o => o.GradeId).FirstAsync();

        var grade = await _ctx.Grades.AsNoTracking()
            .Include(g => g.Curso)
            .Include(g => g.Disciplinas)
            .Include(g => g.Vinculos)
            .FirstAsync(g => g.Id == gradeId);

        return grade.ToOut().Disciplinas.OrderBy(d => d.Periodo).ToList();
    }

    public async Task<List<AlunoOut>> GetAll(Guid faculdadeId)
    {
        var alunos = await _ctx.Alunos
            .AsNoTracking().AsSplitQuery()
            .Include(a => a.Oferta)
                .ThenInclude(o => o.Curso)
            .Where(c => c.FaculdadeId == faculdadeId)
            .ToListAsync();
        
        return alunos.ConvertAll(p => p.ToOut());
    }
}
