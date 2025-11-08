using FluentValidation;

namespace Exato.Front.Features.Office.CriarEmpresa;

public class CriarEmpresaFormData
{
    public string Nome { get; set; }
    public string CNPJ { get; set; }
    public string RazaoSocial { get; set; }
    public bool ExatoWeb { get; set; } = true;

    public static readonly Validator V = new();

    public class Validator : MudAbstractValidator<CriarEmpresaFormData>
    {
        public Validator()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("Informe o nome");

            RuleFor(x => x.RazaoSocial).NotEmpty().WithMessage("Informe a razão social");

            RuleFor(x => x.CNPJ).Must(x => x.IsValidCnpj()).WithMessage("CNPJ inválido");
        }
    }
}
