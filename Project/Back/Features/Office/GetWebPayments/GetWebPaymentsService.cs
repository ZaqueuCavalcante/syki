using Dapper;
using Exato.Web;
using Exato.Shared.Features.Office.GetWebPayments;

namespace Exato.Back.Features.Office.GetWebPayments;

public class GetWebPaymentsService(WebDbContext webCtx) : IOfficeService
{
    public async Task<GetWebPaymentsOut> Get(GetWebPaymentsIn data)
    {
        await webCtx.Database.OpenConnectionAsync();
        var connection = webCtx.Database.GetDbConnection();

        const string totalSql = @"
            SELECT
                count(1)
            FROM
                public.payments p
            INNER JOIN
                public.users u ON u.id = p.user_id
            LEFT JOIN
                public.companies c ON c.id = p.company_id
        ";

        const string itemsSql = @"
            SELECT
                p.id,
                p.transaction_code,
                u.name AS user,
                c.name AS company,
                p.value,
                p.bonus,
                p.start_date,
                p.status,
                p.payment_provider
            FROM
                public.payments p
            INNER JOIN
                public.users u ON u.id = p.user_id
            LEFT JOIN
                public.companies c ON c.id = p.company_id
            ORDER BY
                p.start_date DESC
            LIMIT 10
            OFFSET @Offset
        ";

        var parameters = new
        {
            Offset = data.Page * 10,
        };

        var total = await connection.QueryFirstAsync<int>(totalSql, parameters);
        var payments = await connection.QueryAsync<GetWebPaymentsItemOut>(itemsSql, parameters);

        return new GetWebPaymentsOut()
        {
            Total = total,
            Items = payments.ToList(),
        };
    }
}
