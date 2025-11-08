using Dapper;
using Exato.Back.Intelligence.Domain.Public;
using Exato.Shared.Features.Office.BuscarPotenciaisEmpresasDoUsuario;

namespace Exato.Back.Features.Office.BuscarPotenciaisEmpresasDoUsuario;

public class BuscarPotenciaisEmpresasDoUsuarioService(BackDbContext ctx) : IOfficeService
{
    public async Task<BuscarPotenciaisEmpresasDoUsuarioOut> Get(int id, BuscarPotenciaisEmpresasDoUsuarioIn data)
    {
        await ctx.Database.OpenConnectionAsync();
        var connection = ctx.Database.GetDbConnection();

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
                c.pessoa_fisica = false
                    AND
                c.interno = false
                    AND
                c.cliente_id NOT IN (SELECT u.cliente_id FROM public.organization_users u WHERE u.leaved_at IS NULL AND u.user_id = @Id)
                    AND
                (@Cnpj IS NULL OR c.cpf_cnpj::text ILIKE @Cnpj)
                    AND
                (@Nome IS NULL OR c.nome ILIKE @Nome OR c.razao_social_rf ILIKE @Nome)
            ORDER BY
                c.parent_organization_id DESC, c.ativo DESC, c.cliente_id DESC
            LIMIT 20
        ";

        var termo = data.CnpjOuNome.HasValue() ? data.CnpjOuNome : null;
        var parameters = new
        {
            id,
            Cnpj = termo.CanBeDocument() ? $"%{termo.OnlyNumbers()}%" : null,
            Nome = !termo.CanBeDocument() && termo.HasValue() ? $"%{termo.Trim()}%" : null,
        };

        var empresas = (await connection.QueryAsync<Cliente>(itemsSql, parameters)).ToList();

        return new BuscarPotenciaisEmpresasDoUsuarioOut()
        {
            Items = empresas.ConvertAll(x => new BuscarPotenciaisEmpresasDoUsuarioItemOut()
            {
                Nome = x.Nome,
                Ativa = x.Ativo,
                Id = x.ClienteId,
                Tipo = x.GetTipo(),
                CNPJ = x.CpfCnpj?.ToString() ?? "-",
            })
        };
    }
}
