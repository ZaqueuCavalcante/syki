using Exato.Web;
using Exato.Web.Domain;
using Exato.Shared.Features.Office.CreateCompany;

namespace Exato.Back.Features.Office.CreateCompany;

public class CreateCompanyService(BackDbContext ctx, WebDbContext webCtx) : IOfficeService
{
    public async Task<OneOf<CreateCompanyOut, ExatoError>> Create(CreateCompanyIn data)
    {
        var empresa = await ctx.PublicCliente.AsNoTracking().FirstOrDefaultAsync(x => x.ExternalId == data.ExternalId);
        if (empresa == null) return EmpresaNaoEncontrada.I;

        var paymentMode = empresa.FaturamentoTipoId == null ?
            CompanyPaymentMode.PrePago : (CompanyPaymentMode)(empresa.FaturamentoTipoId.Value - 1);

        var companyDb = await webCtx.Companies.FirstOrDefaultAsync(x => x.ExternalId == empresa.ExternalId);
        if (companyDb != null) return new CreateCompanyOut() { Id = companyDb.Id };

        var company = new Company(
            empresa.ExternalId,
            empresa.GetDocument(),
            empresa.Nome,
            paymentMode
        );

        await webCtx.SaveChangesAsync(company);

        return new CreateCompanyOut() { Id = company.Id };
    }
}
