using FluentValidation;

namespace Exato.Front.Features.Office.EditarCadastroDoUsuario;

public class EditarCadastroDoUsuarioFormData
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string? Cpf { get; set; }

    public static readonly Validator V = new();

    public class Validator : MudAbstractValidator<EditarCadastroDoUsuarioFormData>
    {
        public Validator()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("Informe o nome");

            RuleFor(x => x.Email).Must(x => x.IsValidEmail()).WithMessage("Email inválido");

            When(x => x.Cpf.HasValue(), () =>
            {
                RuleFor(x => x.Cpf).Must(x => x!.IsValidCpf()).WithMessage("CPF inválido");
            });
        }
    }
}
