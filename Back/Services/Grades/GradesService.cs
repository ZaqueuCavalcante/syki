using Syki.Shared;
using Syki.Back.Domain;
using Syki.Back.Database;
using Syki.Back.Exceptions;
using Syki.Back.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Services;

public class GradesService : IGradesService
{
    private readonly SykiDbContext _ctx;
    public GradesService(SykiDbContext ctx) => _ctx = ctx;

    public async Task<GradeOut> Create(Guid faculdadeId, GradeIn data)
    {
        var cursoValido = await _ctx.Cursos
            .AnyAsync(c => c.FaculdadeId == faculdadeId && c.Id == data.CursoId);

        if (!cursoValido)
            throw DomainExceptions.DE0001();

        var grade = new Grade(
            faculdadeId,
            data.CursoId,
            data.Nome
        );

        var disciplinas = await _ctx.CursosDisciplinas.AsNoTracking()
            .Where(x => x.CursoId == data.CursoId)
            .Select(x => x.DisciplinaId)
            .ToListAsync();

        if (!data.Disciplinas.ConvertAll(d => d.Id).IsSubsetOf(disciplinas))
            throw new DomainException(ExceptionMessages.DE0002);

        data.Disciplinas.ForEach(d => grade.Vinculos.Add(
            new GradeDisciplina(d.Id, d.Periodo, d.Creditos, d.CargaHoraria)));

        _ctx.Grades.Add(grade);
        await _ctx.SaveChangesAsync();

        grade = await _ctx.Grades.AsNoTracking()
            .Include(g => g.Curso)
            .Include(x => x.Disciplinas)
            .Include(g => g.Vinculos)
            .FirstAsync(x => x.Id == grade.Id);

        return grade.ToOut();
    }

    public async Task<List<GradeOut>> GetAll(Guid faculdadeId)
    {
        var grades = await _ctx.Grades.AsNoTracking()
            .Where(c => c.FaculdadeId == faculdadeId)
            .Include(g => g.Curso)
            .Include(g => g.Disciplinas)
            .Include(g => g.Vinculos)
            .OrderBy(g => g.Nome)
            .ToListAsync();

        return grades.ConvertAll(g => g.ToOut());
    }
}
