using Exato.Back.Intelligence.Domain.Public;
using Exato.Shared.Features.Office.CriarTokenDeAcesso;

namespace Exato.Back.Features.Office.CriarTokenDeAcesso;

public class CriarTokenDeAcessoService(BackDbContext ctx) : IOfficeService
{
    private class Validator : AbstractValidator<CriarTokenDeAcessoIn>
    {
        public Validator()
        {
            When(x => x.KeyType == TokenAcessoKeyType.Type1, () =>
            {
                RuleFor(x => x.Name).NotEmpty().WithError(NomeDeTokenInvalido.I);
                RuleFor(x => x.Description).NotEmpty().WithError(DescricaoDeTokenInvalida.I);
            });
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CriarTokenDeAcessoOut, ExatoError>> Create(CriarTokenDeAcessoIn data)
    {
        if (V.Run(data, out var error)) return error;

        var empresaOk = await ctx.PublicCliente.AnyAsync(x => x.ClienteId == data.ClienteId);
        if (!empresaOk) return EmpresaNaoEncontrada.I;

        var token = new TokenAcesso(
            data.ClienteId,
            data.KeyType,
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

        if (data.KeyType == TokenAcessoKeyType.Type1)
        {
            var docCheckToken = new DoccheckTokenSetting(token.KeyId!.Value);
            ctx.Add(docCheckToken);
        }

        await ctx.SaveChangesAsync(token);

        return new CriarTokenDeAcessoOut() { Id = token.TokenAcessoId };
    }
}
