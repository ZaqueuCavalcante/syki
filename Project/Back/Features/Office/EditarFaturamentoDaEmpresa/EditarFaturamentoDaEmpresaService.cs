using Exato.Web;
using Exato.Back.Intelligence.Domain.Faturamento;
using Exato.Shared.Features.Office.EditarFaturamentoDaEmpresa;

namespace Exato.Back.Features.Office.EditarFaturamentoDaEmpresa;

public class EditarFaturamentoDaEmpresaService(BackDbContext ctx, WebDbContext webCtx) : IOfficeService
{
    private class Validator : AbstractValidator<EditarFaturamentoDaEmpresaIn>
    {
        public Validator()
        {
            RuleFor(x => x.MetodoDePagamento).IsInEnum().WithError(MetodoDePagamentoInvalido.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<EditarFaturamentoDaEmpresaOut, ExatoError>> Editar(int id, EditarFaturamentoDaEmpresaIn data)
    {
        if (V.Run(data, out var error)) return error;

        var empresa = await ctx.PublicCliente.Where(x => x.ClienteId == id).FirstOrDefaultAsync();
        if (empresa == null) return EmpresaNaoEncontrada.I;

        if (empresa.IsFilial && data.Habilitado) return ApenasMatrizesPodemSerFaturadas.I;

        if (data.MetodoDePagamento == MetodoDePagamento.PrePago && data.Habilitado) return ApenasMatrizesPosPagasPodemSerFaturadas.I;

        if (empresa.IsMatriz && data.Habilitado)
        {
            var configExists = await ctx.FaturamentoClienteConfig
                .AnyAsync(x => x.ClienteId == empresa.ClienteId);

            if (!configExists) ctx.Add(new ClienteConfig(empresa.ClienteId));
        }

        empresa.EditarFaturamento(
            data.Habilitado,
            data.MetodoDePagamento
        );

        await ctx.SaveChangesAsync();

        var company = await webCtx.Companies.FirstOrDefaultAsync(x => x.ExternalId == empresa.ExternalId);
        if (company != null)
        {
            var paymentMode = empresa.FaturamentoTipoId == null ?
                CompanyPaymentMode.PrePago : (CompanyPaymentMode)(empresa.FaturamentoTipoId.Value - 1);

            company.EditarMetodoDePagamento(paymentMode);
            await webCtx.SaveChangesAsync();
        }

        return new EditarFaturamentoDaEmpresaOut() { Id = empresa.ClienteId };
    }
}
