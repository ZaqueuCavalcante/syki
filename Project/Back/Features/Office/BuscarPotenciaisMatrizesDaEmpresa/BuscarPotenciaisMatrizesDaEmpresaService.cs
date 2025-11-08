using Dapper;
using Exato.Back.Intelligence.Domain.Public;
using Exato.Shared.Features.Office.BuscarPotenciaisMatrizesDaEmpresa;

namespace Exato.Back.Features.Office.BuscarPotenciaisMatrizesDaEmpresa;

public class BuscarPotenciaisMatrizesDaEmpresaService(BackDbContext ctx) : IOfficeService
{
    public async Task<BuscarPotenciaisMatrizesDaEmpresaOut> Get(int id, BuscarPotenciaisMatrizesDaEmpresaIn data)
    {
        await ctx.Database.OpenConnectionAsync();
        var connection = ctx.Database.GetDbConnection();

        const string itemsSql = @"
            SELECT
                cliente_id,
                ativo,
                cpf_cnpj,
                nome
            FROM
                public.cliente
            WHERE
                pessoa_fisica = false
                    AND
                cliente_id <> @Id
                    AND
                cliente_id NOT IN (SELECT public.dd_get_all_child_cliente_ids(@Id))
                    AND
                (@Cnpj IS NULL OR cpf_cnpj::text ILIKE @Cnpj)
                    AND
                (@Nome IS NULL OR nome ILIKE @Nome OR razao_social_rf ILIKE @Nome)
            ORDER BY
                parent_organization_id DESC, ativo DESC, cliente_id DESC
            LIMIT 20
        ";

        var termo = data.CnpjOuNome.HasValue() ? data.CnpjOuNome : null;
        var parameters = new
        {
            id,
            Cnpj = termo.CanBeDocument() ? $"%{termo.OnlyNumbers()}%" : null,
            Nome = !termo.CanBeDocument() && termo.HasValue() ? $"%{termo.Trim()}%" : null,
        };

        var matrizes = (await connection.QueryAsync<Cliente>(itemsSql, parameters)).ToList();

        return new BuscarPotenciaisMatrizesDaEmpresaOut()
        {
            Items = matrizes.ConvertAll(x => new BuscarPotenciaisMatrizesDaEmpresaItemOut()
            {
                Nome = x.Nome,
                Ativa = x.Ativo,
                Id = x.ClienteId,
                CNPJ = x.CpfCnpj?.ToString() ?? "-",
            })
        };
    }
}
