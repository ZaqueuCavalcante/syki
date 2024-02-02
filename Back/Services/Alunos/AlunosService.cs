using Syki.Shared;
using Syki.Back.Domain;
using Syki.Back.Configs;
using Syki.Back.Database;
using Syki.Back.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Services;

public class AlunosService : IAlunosService
{
    private readonly SykiDbContext _ctx;
    private readonly IAuthService _authService;
    public AlunosService(
        SykiDbContext ctx,
        IAuthService authService
    ) {
        _ctx = ctx;
        _authService = authService;
    }

    public async Task<AlunoOut> Create(Guid faculdadeId, AlunoIn data)
    {
        using var transaction = _ctx.Database.BeginTransaction();

        var ofertaOk = await _ctx.Ofertas
            .AnyAsync(o => o.FaculdadeId == faculdadeId && o.Id == data.OfertaId);
        if (!ofertaOk)
            Throw.DE0009.Now();

        var userIn = new UserIn
        {
            Faculdade = faculdadeId,
            Name = data.Nome,
            Email = data.Email,
            Password = $"Aluno@{Guid.NewGuid().ToString().OnlyNumbers()}",
            Role = AuthorizationConfigs.Aluno,
        };
        var user = await _authService.Register(userIn);

        var aluno = new Aluno(faculdadeId, user.Id, data.Nome, data.OfertaId);

        _ctx.Add(aluno);
        await _ctx.SaveChangesAsync();

        transaction.Commit();

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
            .Include(a => a.User)
            .Include(a => a.Oferta)
                .ThenInclude(o => o.Curso)
            .Where(c => c.FaculdadeId == faculdadeId)
            .ToListAsync();
        
        return alunos.ConvertAll(p => p.ToOut());
    }
}
