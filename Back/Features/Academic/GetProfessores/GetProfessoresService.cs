namespace Syki.Back.GetProfessores;

public class GetProfessoresService(SykiDbContext ctx)
{
    public async Task<List<ProfessorOut>> Get(Guid institutionId)
    {
        FormattableString sql = $@"
            SELECT
                p.id,
                p.name,
                u.email
            FROM
                syki.professores p
            INNER JOIN
                syki.users u ON u.id = p.id
            WHERE
                u.institution_id = {institutionId}
        ";

        var professores = await ctx.Database.SqlQuery<ProfessorOut>(sql).ToListAsync();

        return professores;
    }
}
