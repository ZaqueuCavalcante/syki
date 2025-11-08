using Exato.Shared.Features.Office.EditarRelatoriosDaEmpresa;

namespace Exato.Back.Features.Office.EditarRelatoriosDaEmpresa;

public class EditarRelatoriosDaEmpresaService(BackDbContext ctx) : IOfficeService
{
    private class Validator : AbstractValidator<EditarRelatoriosDaEmpresaIn>
    {
        public Validator()
        {
            RuleFor(x => x.Pf).NotEmpty().WithError(RelatorioInvalido.I);
            RuleFor(x => x.Pj).NotEmpty().WithError(RelatorioInvalido.I);
            RuleFor(x => x.PfQuod).NotEmpty().WithError(RelatorioInvalido.I);
            RuleFor(x => x.PjQuod).NotEmpty().WithError(RelatorioInvalido.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<EditarRelatoriosDaEmpresaOut, ExatoError>> Editar(int id, EditarRelatoriosDaEmpresaIn data)
    {
        if (V.Run(data, out var error)) return error;

        var empresa = await ctx.PublicCliente.Where(x => x.ClienteId == id).FirstOrDefaultAsync();
        if (empresa == null) return EmpresaNaoEncontrada.I;

        empresa.EditarRelatorios(
            data.Pf,
            data.Pj,
            data.PfQuod,
            data.PjQuod
        );
    
        await ctx.SaveChangesAsync();

        return new EditarRelatoriosDaEmpresaOut() { Id = empresa.ClienteId };
    }
}
