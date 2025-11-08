namespace Exato.Shared.Features.Office.EditarTokenDeAcesso;

public class EditarTokenDeAcessoIn : IApiDto<EditarTokenDeAcessoIn>
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public DateTime? ValidoAte { get; set; }

    public int? TransLimitPerHour { get; set; }
    public int? TransLimitPerDay { get; set; }
    public int? TransLimitPerWeek { get; set; }
    public int? TransLimitPerMonth { get; set; }

    public int? CreditsLimitPerHour { get; set; }
    public int? CreditsLimitPerDay { get; set; }
    public int? CreditsLimitPerWeek { get; set; }
    public int? CreditsLimitPerMonth { get; set; }

    public decimal? CurrencyLimitPerHour { get; set; }
    public decimal? CurrencyLimitPerDay { get; set; }
    public decimal? CurrencyLimitPerWeek { get; set; }
    public decimal? CurrencyLimitPerMonth { get; set; }

    public static IEnumerable<(string, EditarTokenDeAcessoIn)> GetExamples() =>
    [
        ("Exemplo", new EditarTokenDeAcessoIn() { }),
    ];
}
