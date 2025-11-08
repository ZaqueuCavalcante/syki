using Exato.Web;
using Exato.Web.Domain;
using Exato.Back.Intelligence.Domain.Public;
using Exato.Shared.Features.Office.CriarEmpresa;

namespace Exato.Back.Features.Office.CriarEmpresa;

public class CriarEmpresaService(BackDbContext ctx, WebDbContext webCtx) : IOfficeService
{
    private class Validator : AbstractValidator<CriarEmpresaIn>
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

    public async Task<OneOf<CriarEmpresaOut, ExatoError>> Create(CriarEmpresaIn data)
    {
        if (V.Run(data, out var error)) return error;

        var empresa = new Cliente(data.Nome, data.RazaoSocial, data.CNPJ);
        await ctx.SaveChangesAsync(empresa);

        if (data.ExatoWeb)
        {
            var paymentMode = empresa.FaturamentoTipoId == null ?
                CompanyPaymentMode.PrePago : (CompanyPaymentMode)(empresa.FaturamentoTipoId.Value - 1);

            var company = new Company(
                empresa.ExternalId,
                empresa.GetDocument(),
                empresa.Nome,
                paymentMode
            );
            await webCtx.SaveChangesAsync(company);
        }

        return new CriarEmpresaOut() { Id = empresa.ClienteId, ExternalId = empresa.ExternalId };
    }
}
