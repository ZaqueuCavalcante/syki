using Exato.Shared.Features.Office.CriarTokenDeAcesso;

namespace Exato.Front.Features.Office.CriarTokenDeAcesso;

public class CriarTokenDeAcessoClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<CriarTokenDeAcessoOut, ErrorOut>> Criar(
        int clienteId,
        TokenAcessoKeyType keyType,
        string? name,
        string? description,
        DateTime? validoAte,
        int? transLimitPerHour,
        int? transLimitPerDay,
        int? transLimitPerWeek,
        int? transLimitPerMonth,
        int? creditsLimitPerHour,
        int? creditsLimitPerDay,
        int? creditsLimitPerWeek,
        int? creditsLimitPerMonth,
        decimal? currencyLimitPerHour,
        decimal? currencyLimitPerDay,
        decimal? currencyLimitPerWeek,
        decimal? currencyLimitPerMonth)
    {
        var body = new CriarTokenDeAcessoIn
        {
            ClienteId = clienteId,
            KeyType = keyType,
            Name = name,
            Description = description,
            ValidoAte = validoAte,
            TransLimitPerHour = transLimitPerHour,
            TransLimitPerDay = transLimitPerDay,
            TransLimitPerWeek = transLimitPerWeek,
            TransLimitPerMonth = transLimitPerMonth,
            CreditsLimitPerHour = creditsLimitPerHour,
            CreditsLimitPerDay = creditsLimitPerDay,
            CreditsLimitPerWeek = creditsLimitPerWeek,
            CreditsLimitPerMonth = creditsLimitPerMonth,
            CurrencyLimitPerHour = currencyLimitPerHour,
            CurrencyLimitPerDay = currencyLimitPerDay,
            CurrencyLimitPerWeek = currencyLimitPerWeek,
            CurrencyLimitPerMonth = currencyLimitPerMonth,
        };

        var response = await http.PostAsJsonAsync("office/tokens", body);

        return await response.Resolve<CriarTokenDeAcessoOut>();
    }
}
