using Exato.Shared.Features.Office.EditarSaldoDaEmpresa;

namespace Exato.Back.Features.Office.EditarSaldoDaEmpresa;

public class EditarSaldoDaEmpresaService(BackDbContext ctx) : IOfficeService
{
    public async Task<OneOf<EditarSaldoDaEmpresaOut, ExatoError>> Editar(int id, EditarSaldoDaEmpresaIn data)
    {
        await ctx.Database.BeginTransactionAsync();

        var empresa = await ctx.PublicCliente
            .FromSql($"SELECT * FROM public.cliente WHERE cliente_id = {id} FOR UPDATE")
            .FirstOrDefaultAsync();
        if (empresa == null) return EmpresaNaoEncontrada.I;

        if (empresa.IsPosPago) return MetodoDePagamentoInvalido.I;

        if (empresa.IsReais)
        {
            if (data.Amount == 0) return ValorEmReaisInvalido.I;
            empresa.EditarSaldo(data.Amount);
        }

        if (empresa.IsCreditos)
        {
            if (data.Credits == 0) return ValorEmCreditosInvalido.I;
            empresa.EditarCreditos(data.Credits);
        }

        await ctx.SaveChangesAsync();

        await ctx.Database.CommitTransactionAsync();

        return new EditarSaldoDaEmpresaOut() { Id = empresa.ClienteId };
    }
}
