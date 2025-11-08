using Exato.Shared.Features.Office.EditarTokenDeAcesso;

namespace Exato.Back.Features.Office.EditarTokenDeAcesso;

public class EditarTokenDeAcessoService(BackDbContext ctx) : ICrossService
{
    public async Task<OneOf<EditarTokenDeAcessoOut, ExatoError>> Editar(int id, EditarTokenDeAcessoIn data)
    {
        var token = await ctx.PublicTokenAcesso.FirstOrDefaultAsync(x => x.TokenAcessoId == id);
        if (token is null) return TokenDeAcessoNaoEncontrado.I;

        if (data.ValidoAte is DateTime validoAte)
        {
            if (validoAte < DateTime.Now.AddDays(-1)) return ValidadeDeTokenInvalida.I;
        }

        if ((TokenAcessoKeyType)token.KeyType == TokenAcessoKeyType.Type1)
        {
            if (data.Name.IsEmpty()) return NomeDeTokenInvalido.I;
            if (data.Description.IsEmpty()) return DescricaoDeTokenInvalida.I;
        }

        token.Editar(
            data.Name,
            data.Description,
            data.ValidoAte,
            data.TransLimitPerHour,
            data.TransLimitPerDay,
            data.TransLimitPerWeek,
            data.TransLimitPerMonth,
            data.CreditsLimitPerHour,
            data.CreditsLimitPerDay,
            data.CreditsLimitPerWeek,
            data.CreditsLimitPerMonth,
            data.CurrencyLimitPerHour,
            data.CurrencyLimitPerDay,
            data.CurrencyLimitPerWeek,
            data.CurrencyLimitPerMonth
        );

        await ctx.SaveChangesAsync();

        return new EditarTokenDeAcessoOut() { Id = token.TokenAcessoId };
    }
}
