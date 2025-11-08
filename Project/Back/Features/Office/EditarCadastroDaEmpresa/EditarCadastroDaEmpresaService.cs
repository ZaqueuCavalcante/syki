using Dapper;
using Exato.Web;
using Exato.Shared.Features.Office.EditarCadastroDaEmpresa;

namespace Exato.Back.Features.Office.EditarCadastroDaEmpresa;

public class EditarCadastroDaEmpresaService(BackDbContext ctx, WebDbContext webCtx) : IOfficeService
{
    private class Validator : AbstractValidator<EditarCadastroDaEmpresaIn>
    {
        public Validator()
        {
            RuleFor(x => x.Nome).NotEmpty().WithError(NomeDeEmpresaInvalido.I);
            RuleFor(x => x.Nome).MaximumLength(150).WithError(NomeDeEmpresaInvalido.I);

            RuleFor(x => x.CNPJ).Must(x => x.IsValidCnpj()).WithError(InvalidCnpj.I);

            RuleFor(x => x.RazaoSocial).NotEmpty().WithError(NomeDeEmpresaInvalido.I);
            RuleFor(x => x.RazaoSocial).MaximumLength(150).WithError(NomeDeEmpresaInvalido.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<EditarCadastroDaEmpresaOut, ExatoError>> Editar(int id, EditarCadastroDaEmpresaIn data)
    {
        if (V.Run(data, out var error)) return error;

        var empresa = await ctx.PublicCliente.Where(x => x.ClienteId == id).FirstOrDefaultAsync();
        if (empresa == null) return EmpresaNaoEncontrada.I;

        if (data.MatrizId != null)
        {
            var matrizes = await BuscarPotenciaisMatrizes(id);
            if (!matrizes.Any(x => x == data.MatrizId)) return MatrizInvalida.I;
        }

        if (data.SalesContact.HasValue() && data.MatrizId != null)
            return ApenasMatrizesPodemPossuirVendedorResponsavel.I;

        empresa.EditarCadastro(
            data.Ativa,
            data.Nome,
            data.CNPJ,
            data.RazaoSocial,
            data.MatrizId,
            data.NomeFantasia,
            data.Slug,
            data.SalesContact
        );

        await ctx.SaveChangesAsync();

        var company = await webCtx.Companies.FirstOrDefaultAsync(x => x.ExternalId == empresa.ExternalId);
        if (company != null)
        {
            company.EditarCadastro(data.Ativa, data.CNPJ.OnlyNumbers(), data.Nome);
            await webCtx.SaveChangesAsync();
        }

        return new EditarCadastroDaEmpresaOut() { Id = empresa.ClienteId };
    }

    private async Task<IEnumerable<int>> BuscarPotenciaisMatrizes(int id)
    {
        var connection = ctx.Database.GetDbConnection();

        const string itemsSql = @"
            SELECT
                cliente_id
            FROM
                public.cliente
            WHERE
                pessoa_fisica = false
                    AND
                cliente_id <> @Id
                    AND
                cliente_id NOT IN (SELECT public.dd_get_all_child_cliente_ids(@Id))
        ";

        return await connection.QueryAsync<int>(itemsSql, new { id });
    }
}
