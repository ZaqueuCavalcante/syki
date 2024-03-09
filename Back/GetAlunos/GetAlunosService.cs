namespace Syki.Back.GetAlunos;

public class GetAlunosService(SykiDbContext ctx)
{
    public async Task<List<AlunoOut>> Get(Guid institutionId)
    {
        var alunos = await ctx.Alunos
            .AsNoTracking().AsSplitQuery()
            .Include(a => a.User)
            .Include(a => a.Oferta)
                .ThenInclude(o => o.Curso)
            .Where(a => a.InstitutionId == institutionId)
            .ToListAsync();
        
        return alunos.ConvertAll(p => p.ToOut());
    }
}
