namespace Exato.Shared.Features.Office.BuscarTokensDeAcesso;

public class BuscarTokensDeAcessoOut : IApiDto<BuscarTokensDeAcessoOut>
{
    public int Total { get; set; }
    public List<BuscarTokensDeAcessoItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, BuscarTokensDeAcessoOut)> GetExamples() =>
    [
        ("Exemplo", new BuscarTokensDeAcessoOut() { }),
    ];
}

public class BuscarTokensDeAcessoItemOut
{
    public int Id { get; set; }
    public TokenAcessoKeyType KeyType { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string Token { get; set; }
    public string? KeyId { get; set; }
    public string? SecretHash { get; set; }
    public DateTime IncluidoEm { get; set; }
    public DateTime? ValidoAte { get; set; }

    public int? UsuarioId { get; set; }
    public string? Usuario { get; set; }
    public DateTime? ExcluidoEm { get; set; }

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
}
