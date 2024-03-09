namespace Syki.Back.Services;

public class ProfessoresService : IProfessoresService
{
    private readonly SykiDbContext _ctx;
    private readonly IAuthService _authService;
    public ProfessoresService(
        SykiDbContext ctx,
        IAuthService authService
    ) {
        _ctx = ctx;
        _authService = authService;
    }

    public async Task<ProfessorOut> Create(Guid faculdadeId, ProfessorIn data)
    {
        using var transaction = _ctx.Database.BeginTransaction();

        var userIn = new CreateUserIn
        {
            InstitutionId = faculdadeId,
            Name = data.Nome,
            Email = data.Email,
            Password = $"Professor@{Guid.NewGuid().ToString().OnlyNumbers()}",
            Role = AuthorizationConfigs.Professor,
        };
        var user = await _authService.Register(userIn);

        var professor = new Professor(user.Id, faculdadeId, data.Nome);

        _ctx.Add(professor);
        await _ctx.SaveChangesAsync();

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

        var professores = await _ctx.Database.SqlQuery<ProfessorOut>(sql).ToListAsync();

        return professores;
    }
}
