using Dapper;
using Exato.Back.Intelligence.Domain.Public;
using Exato.Shared.Features.Office.BuscarUsuariosDaEmpresa;

namespace Exato.Back.Features.Office.BuscarUsuariosDaEmpresa;

public class BuscarUsuariosDaEmpresaService(BackDbContext ctx) : IOfficeService
{
    public async Task<BuscarUsuariosDaEmpresaOut> Get(int id, BuscarUsuariosDaEmpresaIn data)
    {
        await ctx.Database.OpenConnectionAsync();
        var connection = ctx.Database.GetDbConnection();

        const string totalSql = @"
            SELECT
                count(1)
            FROM
                public.users u
            INNER JOIN
                public.organization_users o ON o.user_id = u.id
            WHERE
                o.cliente_id = @Id
                    AND
                o.leaved_at IS NULL
                    AND
                (@IsActive IS NULL OR u.active = @IsActive)
                    AND
                (@Doc IS NULL OR u.cpf::text ILIKE @Doc)
                    AND
                (@Email IS NULL OR u.email ILIKE @Email)
                    AND
                (@Name IS NULL OR u.full_name ILIKE @Name)
        ";

        const string itemsSql = @"
            SELECT
                u.id,
                u.full_name,
                u.email,
                u.cpf::text,
                u.created_at,
                u.active
            FROM
                public.users u
            INNER JOIN
                public.organization_users o ON o.user_id = u.id
            WHERE
                o.cliente_id = @Id
                    AND
                o.leaved_at IS NULL
                    AND
                (@IsActive IS NULL OR u.active = @IsActive)
                    AND
                (@Doc IS NULL OR u.cpf::text ILIKE @Doc)
                    AND
                (@Email IS NULL OR u.email ILIKE @Email)
                    AND
                (@Name IS NULL OR u.full_name ILIKE @Name)
            ORDER BY
                u.created_at DESC
            LIMIT 10
            OFFSET @Offset
        ";

        var termo = data.SearchTerm.HasValue() ? data.SearchTerm : null;
        var parameters = new
        {
            id,
            data.IsActive,
            Offset = data.Page * 10,
            Doc = termo.CanBeDocument() ? $"%{termo.OnlyNumbers()}%" : null,
            Email = !termo.CanBeDocument() && termo.CanBeEmail() ? $"%{termo.Trim()}%" : null,
            Name = !termo.CanBeDocument() && !termo.CanBeEmail() && termo.HasValue() ? $"%{termo.Trim()}%" : null,
        };

        var total = await connection.QueryFirstAsync<int>(totalSql, parameters);
        var usuarios = (await connection.QueryAsync<User>(itemsSql, parameters)).ToList();

        return new BuscarUsuariosDaEmpresaOut()
        {
            Total = total,
            Items = usuarios.ConvertAll(x => 
                new BuscarUsuariosDaEmpresaItemOut()
                    {
                        Id = x.Id,
                        Nome = x.FullName ?? "-",
                        Email = x.Email ?? "-",
                        Documento = x.GetCpf() ?? "-",
                        CriadoEm = x.CreatedAt,
                        Ativo = x.Active,
                    }
                )
        };
    }
}
