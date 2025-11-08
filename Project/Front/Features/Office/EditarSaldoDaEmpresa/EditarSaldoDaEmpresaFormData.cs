using FluentValidation;

namespace Exato.Front.Features.Office.EditarSaldoDaEmpresa;

public class EditarSaldoDaEmpresaFormData
{
    public int Id { get; set; }
    public bool IsUp { get; set; }
    public decimal Amount { get; set; }
    public int? Credits { get; set; }
    public BalanceType BalanceType { get; set; }

    public decimal GetAmount()
    {
        return IsUp ? Amount : -Amount;
    }

    public int GetCredits()
    {
        if (Credits == null) return 0;
        return IsUp ? Credits.Value : -Credits.Value;
    }

    public static readonly Validator V = new();

    public class Validator : MudAbstractValidator<EditarSaldoDaEmpresaFormData>
    {
        public Validator()
        {
            When(x => x.BalanceType == BalanceType.Reais, () =>
            {
                RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Informe um valor maior que 0");
            });

            When(x => x.BalanceType == BalanceType.Creditos, () =>
            {
                RuleFor(x => x.Credits).GreaterThan(0).WithMessage("Informe um valor maior que 0");
            });
        }
    }
}
