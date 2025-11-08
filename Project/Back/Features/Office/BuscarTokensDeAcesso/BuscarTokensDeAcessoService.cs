using Dapper;
using Exato.Shared.Features.Office.BuscarTokensDeAcesso;

namespace Exato.Back.Features.Office.BuscarTokensDeAcesso;

public class BuscarTokensDeAcessoService(BackDbContext ctx) : IOfficeService
{
    public async Task<BuscarTokensDeAcessoOut> Get(BuscarTokensDeAcessoIn data)
    {
        await ctx.Database.OpenConnectionAsync();
        var connection = ctx.Database.GetDbConnection();

        const string totalSql = @"
            SELECT
                count(1)
            FROM
                public.token_acesso t
            WHERE
                t.cliente_id = @ClienteId
                    AND
                (@KeyType IS NULL OR t.key_type = @KeyType)
        ";

        const string itemsSql = @"
            SELECT
                t.name,
                t.token,
                t.key_type,
                t.usuario_id,
                u.full_name AS usuario,
                t.valido_ate,
                t.excluido_em,
                t.secret_hash,
                t.incluido_em,
                t.description,
                t.token_acesso_id,
                t.trans_limit_per_hour,
                t.trans_limit_per_day,
                t.trans_limit_per_week,
                t.trans_limit_per_month,
                t.credits_limit_per_hour,
                t.credits_limit_per_day,
                t.credits_limit_per_week,
                t.credits_limit_per_month,
                t.currency_limit_per_hour,
                t.currency_limit_per_day,
                t.currency_limit_per_week,
                t.currency_limit_per_month
            FROM
                public.token_acesso t
            LEFT JOIN
                public.users u ON u.id = t.usuario_id
            WHERE
                t.cliente_id = @ClienteId
                    AND
                (@KeyType IS NULL OR t.key_type = @KeyType)
            ORDER BY
                t.incluido_em DESC
            LIMIT 10
            OFFSET @Offset
        ";

        var parameters = new
        {
            data.ClienteId,
            Offset = data.Page * 10,
            KeyType = data.KeyType?.ToShort() ?? null,
        };

        var total = await connection.QueryFirstAsync<int>(totalSql, parameters);
        var tokens = (await connection.QueryAsync<BuscarTokensDeAcessoItemOut>(itemsSql, parameters)).ToList();

        tokens.ForEach(x => x.Token = x.Token.ToMaskedToken());

        return new BuscarTokensDeAcessoOut()
        {
            Total = total,
            Items = tokens,
        };
    }
}
