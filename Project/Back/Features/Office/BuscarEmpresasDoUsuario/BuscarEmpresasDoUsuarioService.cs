using Dapper;
using Exato.Back.Intelligence.Domain.Public;
using Exato.Shared.Features.Office.BuscarEmpresasDoUsuario;

namespace Exato.Back.Features.Office.BuscarEmpresasDoUsuario;

public class BuscarEmpresasDoUsuarioService(BackDbContext ctx) : IOfficeService
{
    public async Task<BuscarEmpresasDoUsuarioOut> Get(int userId, BuscarEmpresasDoUsuarioIn data)
    {
        await ctx.Database.OpenConnectionAsync();
        var connection = ctx.Database.GetDbConnection();

        const string totalSql = @"
            SELECT
                count(1)
            FROM
                public.cliente c
            WHERE
                c.cliente_id IN (SELECT u.cliente_id FROM public.organization_users u WHERE NOT u.its_his_own AND u.leaved_at IS NULL AND u.user_id = @UserId)
        ";

        const string itemsSql = @"
            SELECT
                c.cliente_id,
                c.ativo,
                c.cpf_cnpj,
                c.nome,
                c.parent_organization_id,
                EXISTS(SELECT 1 FROM public.cliente f WHERE f.parent_organization_id = c.cliente_id) AS is_parent
            FROM
                public.cliente c
            WHERE
                c.cliente_id IN (SELECT u.cliente_id FROM public.organization_users u WHERE NOT u.its_his_own AND u.leaved_at IS NULL AND u.user_id = @UserId)
            ORDER BY
                c.incluido_em DESC
            LIMIT 10
            OFFSET @Offset
        ";

        var parameters = new
        {
            userId,
            Offset = data.Page * 10,
        };

        var total = await connection.QueryFirstAsync<int>(totalSql, parameters);
        var empresas = (await connection.QueryAsync<Cliente>(itemsSql, parameters)).ToList();

        return new BuscarEmpresasDoUsuarioOut()
        {
            Total = total,
            Items = empresas.ConvertAll(x => x.ToBuscarEmpresasDoUsuarioItemOut()),
        };
    }
}
