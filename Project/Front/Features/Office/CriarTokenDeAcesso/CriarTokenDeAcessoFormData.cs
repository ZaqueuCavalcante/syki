using FluentValidation;

namespace Exato.Front.Features.Office.CriarTokenDeAcesso;

public class CriarTokenDeAcessoFormData
{
    public int ClienteId { get; set; }
    public TokenAcessoKeyType KeyType { get; set; }

    public MetodoDePagamento MetodoDePagamento { get; set; }
    public BalanceType BalanceType { get; set; }

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

    public DateTime? GetDateTime()
    {
        if (ValidoAte == null) return null;

        var value = ValidoAte.Value;
        return new DateTime(value.Year, value.Month, value.Day, 12, 0, 0, value.Kind);
    }

    public static readonly Validator V = new();

    public class Validator : MudAbstractValidator<CriarTokenDeAcessoFormData>
    {
        public Validator()
        {
            When(x => x.KeyType == TokenAcessoKeyType.Type1, () =>
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage("Informe o nome");
                RuleFor(x => x.Description).NotEmpty().WithMessage("Informe a descrição");
            });
        }
    }
}
