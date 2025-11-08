using FluentValidation;
using Exato.Front.Components.Empresas;

namespace Exato.Front.Features.Office.UpdateRole;

public class UpdateRoleFormData
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Description { get; set; }

    public static readonly Validator V = new();

    public class Validator : MudAbstractValidator<UpdateRoleFormData>
    {
        public Validator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Informe o nome");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Informe a descrição");
        }
    }
}
