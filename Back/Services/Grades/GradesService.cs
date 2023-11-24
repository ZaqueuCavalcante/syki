using Syki.Shared;
using Syki.Back.Domain;
using Syki.Back.Database;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Services;

public class GradesService : IGradesService
{
    private readonly SykiDbContext _ctx;
    public GradesService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<GradeOut> Create(Guid faculdadeId, GradeIn data)
    {
        var grade = new Grade
        {
            Id = Guid.NewGuid(),
            FaculdadeId = faculdadeId,
            CursoId = data.CursoId,
            Nome = data.Nome,
            Vinculos = new(),
        };

        foreach (var d in data.Disciplinas)
        {
            grade.Vinculos.Add(new GradeDisciplina {
                DisciplinaId = d.Id,
                Periodo = d.Periodo,
                Creditos = d.Creditos,
                CargaHoraria = d.CargaHoraria,
            });
        }

        _ctx.Grades.Add(grade);
        await _ctx.SaveChangesAsync();

        grade = await _ctx.Grades.AsNoTracking()
            .Include(g => g.Curso)
            .Include(x => x.Disciplinas)
            .FirstAsync(x => x.Id == grade.Id);

        return grade.ToOut();
    }

    public async Task<List<GradeOut>> GetAll(Guid faculdadeId)
    {
        var grades = await _ctx.Grades
            .Where(c => c.FaculdadeId == faculdadeId)
            .Include(g => g.Curso)
            .Include(g => g.Disciplinas)
            .Include(g => g.Vinculos)
            .ToListAsync();

        return grades.ConvertAll(g => g.ToOut());
    }

    public async Task<List<DisciplinaOut>> GetDisciplinas(Guid faculdadeId, Guid id)
    {
        var grade = await _ctx.Grades
            .Where(c => c.FaculdadeId == faculdadeId && c.Id == id)
            .Include(g => g.Disciplinas)
            .FirstOrDefaultAsync();
        
        if (grade == null) return [];

        return grade.Disciplinas.ConvertAll(g => g.ToOut());
    }
}
