using FluentValidation;
using Exato.Front.Components.Empresas;

namespace Exato.Front.Features.Office.CreateUser;

public class CreateUserFormData
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public EmpresaSelectData Empresa { get; set; } = new();
    public EmpresaRoleSelectData Role { get; set; } = new();

    public static readonly Validator V = new();

    public class Validator : MudAbstractValidator<CreateUserFormData>
    {
        public Validator()
        {
            RuleFor(x => x.Empresa)
                .Must(x => x != null && x.Id != 0)
                .WithMessage("Informe a empresa");

            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Informe o nome");

            RuleFor(x => x.Email)
                .Must(x => x.IsValidEmail())
                .WithMessage("Email inválido");

            RuleFor(x => x.Role)
                .Must(x => x != null && x.Id != Guid.Empty)
                .WithMessage("Informe a role");
        }
    }
}
