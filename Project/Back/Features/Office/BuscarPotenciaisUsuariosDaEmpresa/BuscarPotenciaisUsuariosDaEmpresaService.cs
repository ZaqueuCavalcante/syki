using Dapper;
using Exato.Back.Intelligence.Domain.Public;
using Exato.Shared.Features.Office.BuscarPotenciaisUsuariosDaEmpresa;

namespace Exato.Back.Features.Office.BuscarPotenciaisUsuariosDaEmpresa;

public class BuscarPotenciaisUsuariosDaEmpresaService(BackDbContext ctx) : IOfficeService
{
    public async Task<BuscarPotenciaisUsuariosDaEmpresaOut> Get(int id, BuscarPotenciaisUsuariosDaEmpresaIn data)
    {
        await ctx.Database.OpenConnectionAsync();
        var connection = ctx.Database.GetDbConnection();

        const string itemsSql = @"
            SELECT
                u.id,
                u.email,
                u.active,
                u.full_name
            FROM
                public.users u
            WHERE
                NOT EXISTS (SELECT true FROM public.organization_users o WHERE o.leaved_at IS NULL AND o.cliente_id = @Id AND o.user_id = u.id)
                    AND
                (@Name IS NULL OR u.full_name ILIKE @Name)
                    AND
                (@Email IS NULL OR u.email ILIKE @Email)
            ORDER BY
                u.active DESC, u.id DESC
            LIMIT 20
        ";

        var termo = data.NameOuEmail.HasValue() ? data.NameOuEmail : null;
        var parameters = new
        {
            id,
            Email = termo.CanBeEmail() ? $"%{termo.Trim()}%" : null,
            Name = !termo.CanBeEmail() && termo.HasValue() ? $"%{termo.Trim()}%" : null,
        };

        var usuarios = (await connection.QueryAsync<User>(itemsSql, parameters)).ToList();

        return new BuscarPotenciaisUsuariosDaEmpresaOut()
        {
            Items = usuarios.ConvertAll(x => new BuscarPotenciaisUsuariosDaEmpresaItemOut()
            {
                Id = x.Id,
                Active = x.Active,
                Email = x.Email ?? "-",
                Name = x.FullName ?? "-",
            })
        };
    }
}
