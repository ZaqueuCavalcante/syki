using Exato.Shared.Features.Office.EditarConsultasDaEmpresa;

namespace Exato.Back.Features.Office.EditarConsultasDaEmpresa;

public class EditarConsultasDaEmpresaService(BackDbContext ctx) : IOfficeService
{
    private class Validator : AbstractValidator<EditarConsultasDaEmpresaIn>
    {
        public Validator()
        {
            RuleFor(x => x.DataAccessLevel)
                .IsInEnum()
                .WithError(NivelDeAcessoADadosInvalido.I);

            RuleFor(x => x.TransLimitPerWeek)
                .GreaterThanOrEqualTo(0)
                .When(x => x.TransLimitPerWeek != null)
                .WithError(LimiteDeConsultasSemanalInvalido.I);
        }
    }

    private static readonly Validator V = new();

    public async Task<OneOf<EditarConsultasDaEmpresaOut, ExatoError>> Editar(int id, EditarConsultasDaEmpresaIn data)
    {
        if (V.Run(data, out var error)) return error;

        var empresa = await ctx.PublicCliente.Where(x => x.ClienteId == id).FirstOrDefaultAsync();
        if (empresa == null) return EmpresaNaoEncontrada.I;

        empresa.EditarConsultas(
            data.HighPerformance,
            data.BlockSensitiveDataInQueryString,
            data.DataAccessLevel,
            data.TransLimitPerWeek,
            data.GerarPdfConsultas,
            data.HabilitarConsultasPorEmail,
            data.ReceitaCpfUseSerproAsMainSource,
            data.ReceitaCpfShouldReturnMinor18AgeData
        );

        await ctx.SaveChangesAsync();

        return new EditarConsultasDaEmpresaOut() { Id = empresa.ClienteId };
    }
}
