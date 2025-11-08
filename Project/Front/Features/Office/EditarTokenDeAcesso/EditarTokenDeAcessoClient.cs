using Exato.Shared.Features.Office.EditarTokenDeAcesso;

namespace Exato.Front.Features.Office.EditarTokenDeAcesso;

public class EditarTokenDeAcessoClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<EditarTokenDeAcessoOut, ErrorOut>> Editar(
        int id,
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
        var body = new EditarTokenDeAcessoIn
        {
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

        var response = await http.PutAsJsonAsync($"office/tokens/{id}", body);

        return await response.Resolve<EditarTokenDeAcessoOut>();
    }
}
