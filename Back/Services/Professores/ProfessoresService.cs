namespace Syki.Back.Services;

public class ProfessoresService(SykiDbContext ctx, IAuthService authService) : IProfessoresService
{

    public async Task<ProfessorOut> Create(Guid faculdadeId, ProfessorIn data)
    {
        using var transaction = ctx.Database.BeginTransaction();

        var userIn = new CreateUserIn
        {
            InstitutionId = faculdadeId,
            Name = data.Nome,
            Email = data.Email,
            Password = $"Professor@{Guid.NewGuid().ToString().OnlyNumbers()}",
            Role = AuthorizationConfigs.Professor,
        };
        var user = await authService.Register(userIn);

        var professor = new Professor(user.Id, faculdadeId, data.Nome);

        ctx.Add(professor);
        await ctx.SaveChangesAsync();

        transaction.Commit();

        return professor.ToOut();
    }

    public async Task<List<ProfessorOut>> GetAll(Guid faculdadeId)
    {
        FormattableString sql = $@"
            SELECT
                p.id,
                p.nome,
                u.email
            FROM
                syki.professores p
            INNER JOIN
                syki.users u ON u.id = p.id
            WHERE
                u.institution_id = {faculdadeId}
        ";

        var professores = await ctx.Database.SqlQuery<ProfessorOut>(sql).ToListAsync();

        return professores;
    }
}
