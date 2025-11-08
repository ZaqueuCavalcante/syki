using Dapper;
using Exato.Shared.Features.Office.BuscarFiliaisDaEmpresa;

namespace Exato.Back.Features.Office.BuscarFiliaisDaEmpresa;

public class BuscarFiliaisDaEmpresaService(BackDbContext ctx) : IOfficeService
{
    public async Task<BuscarFiliaisDaEmpresaOut> Get(int id)
    {
        const string sql = @"
            SELECT
                nome,
                cliente_id AS id,
                parent_organization_id AS parentId
            FROM
                public.cliente
            WHERE
                cliente_id IN (SELECT public.dd_get_all_child_cliente_ids(@MasterId))
        ";
        var parameters = new { MasterId = id };

        var connection = ctx.Database.GetDbConnection();
        var filiais = await connection.QueryAsync<EmpresaFilialDto>(sql, parameters) ?? [];

        return new()
        {
            Items = filiais
                .OrdenarHierarquicamente(id)
                .OrderByDescending(x => x.Filiais.Count)
                .ToList(),
        };
    }
}

public class EmpresaFilialDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int ParentId { get; set; }
}
