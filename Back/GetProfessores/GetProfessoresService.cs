namespace Syki.Back.GetProfessores;

public class GetProfessoresService(SykiDbContext ctx)
{
    public async Task<List<ProfessorOut>> Get(Guid faculdadeId)
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
