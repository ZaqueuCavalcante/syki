using FluentValidation;
using Exato.Front.Components.Empresas;

namespace Exato.Front.Features.Office.CreateRole;

public class CreateRoleFormData
{
    public string Nome { get; set; }
    public string Description { get; set; }
    public EmpresaSelectData Empresa { get; set; } = new();

    public static readonly Validator V = new();

    public class Validator : MudAbstractValidator<CreateRoleFormData>
    {
        public Validator()
        {
            RuleFor(x => x.Empresa)
                .Must(x => x != null && x.Id != 0)
                .WithMessage("Informe a empresa");

            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Informe o nome");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Informe a descrição");
        }
    }
}
