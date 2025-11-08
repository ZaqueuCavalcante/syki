using Dapper;
using Exato.Web;
using Exato.Back.Intelligence.Domain.Public;
using Exato.Shared.Features.Office.BuscarEmpresas;

namespace Exato.Back.Features.Office.BuscarEmpresas;

public class BuscarEmpresasService(BackDbContext ctx, WebDbContext webCtx) : IOfficeService
{
    public async Task<BuscarEmpresasOut> Get(BuscarEmpresasIn data)
    {
        await ctx.Database.OpenConnectionAsync();
        var connection = ctx.Database.GetDbConnection();

        const string totalSql = @"
            SELECT
                count(1)
            FROM
                public.cliente c
            WHERE
                c.pessoa_fisica = false
                    AND
                c.faturamento_tipo_id = ANY(@PaymentMethods)
                    AND
                (@OnboardStatus IS NULL OR c.external_id = ANY(@CompaniesIds))
                    AND
                (@IsActive IS NULL OR c.ativo = @IsActive)
                    AND
                (@Cnpj IS NULL OR c.cpf_cnpj::text ILIKE @Cnpj)
                    AND
                (@Name IS NULL OR c.nome ILIKE @Name OR c.razao_social_rf ILIKE @Name)
                    AND
                (@IsFilial IS NULL OR c.parent_organization_id IS NOT NULL)
                    AND
                (@IsMatriz IS NULL OR (c.parent_organization_id IS NULL AND EXISTS(SELECT 1 FROM public.cliente f WHERE f.parent_organization_id = c.cliente_id)))
                    AND
                (@IsAvulsa IS NULL OR (c.parent_organization_id IS NULL AND NOT EXISTS(SELECT 1 FROM public.cliente f WHERE f.parent_organization_id = c.cliente_id)))
        ";

        const string itemsSql = @"
            SELECT
                c.cliente_id,
                c.ativo,
                c.cpf_cnpj,
                c.nome,
                c.razao_social_rf,
                c.incluido_em,
                c.faturamento_tipo_id,
                c.parent_organization_id,
                EXISTS(SELECT 1 FROM public.cliente f WHERE f.parent_organization_id = c.cliente_id) AS is_parent
            FROM
                public.cliente c
            WHERE
                c.pessoa_fisica = false
                    AND
                c.faturamento_tipo_id = ANY(@PaymentMethods)
                    AND
                (@OnboardStatus IS NULL OR c.external_id = ANY(@CompaniesIds))
                    AND
                (@IsActive IS NULL OR c.ativo = @IsActive)
                    AND
                (@Cnpj IS NULL OR c.cpf_cnpj::text ILIKE @Cnpj)
                    AND
                (@Name IS NULL OR c.nome ILIKE @Name OR c.razao_social_rf ILIKE @Name)
                    AND
                (@IsFilial IS NULL OR c.parent_organization_id IS NOT NULL)
                    AND
                (@IsMatriz IS NULL OR (c.parent_organization_id IS NULL AND EXISTS(SELECT 1 FROM public.cliente f WHERE f.parent_organization_id = c.cliente_id)))
                    AND
                (@IsAvulsa IS NULL OR (c.parent_organization_id IS NULL AND NOT EXISTS(SELECT 1 FROM public.cliente f WHERE f.parent_organization_id = c.cliente_id)))
            ORDER BY
                c.incluido_em DESC
            LIMIT 10
            OFFSET @Offset
        ";

        bool? isFilial = data.ClientType == TipoDeEmpresa.Filial ? true : null;
        bool? isMatriz = data.ClientType == TipoDeEmpresa.Matriz ? true : null;
        bool? isAvulsa = data.ClientType == TipoDeEmpresa.Avulsa ? true : null;

        var companiesIds = data.OnboardStatus != null ? await webCtx.Companies
            .Where(x => x.ExternalId != null && x.OnboardStatus == data.OnboardStatus.Value.ToInt())
            .Select(x => x.ExternalId!.Value)
            .ToListAsync() : [];

        var parameters = new
        {
            data.IsActive,
            IsFilial = isFilial,
            IsMatriz = isMatriz,
            IsAvulsa = isAvulsa,
            Offset = data.Page * 10,
            CompaniesIds = companiesIds.ToArray(),
            OnboardStatus = data.OnboardStatus?.ToInt(),
            Name = !data.Search.CanBeDocument() && data.Search.HasValue() ? $"%{data.Search.Trim()}%" : null,
            Cnpj = data.Search.CanBeDocument() ? $"%{data.Search.OnlyNumbers().Trim().TrimStart('0')}%" : null,
            PaymentMethods = data.PaymentMethod == null ? new int[] { 1, 2 } : [Convert.ToInt32(data.PaymentMethod)],
        };

        var total = await connection.QueryFirstAsync<int>(totalSql, parameters);
        var clientes = (await connection.QueryAsync<Cliente>(itemsSql, parameters)).ToList();

        return new BuscarEmpresasOut()
        {
            Total = total,
            Items = clientes.ConvertAll(x => new BuscarEmpresasItemOut()
            {
                Nome = x.Nome,
                Id = x.ClienteId,
                Ativa = x.Ativo,
                Tipo = x.GetTipo(),
                CriadaEm = x.IncluidoEm,
                CNPJ = x.GetDocument() ?? "-",
                RazaoSocial = x.RazaoSocialRf ?? "-",
                MetodoDePagamento = (x.FaturamentoTipoId ?? 1).ToEnum<MetodoDePagamento>(),
            })
        };
    }
}
