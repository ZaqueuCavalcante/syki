using FluentValidation;
using Exato.Front.Components.Empresas;

namespace Exato.Front.Features.Office.VincularEmpresaUsuario;

public class VincularUsuarioAEmpresaFormData
{
    public PotencialUsuarioDaEmpresaSelectData Usuario { get; set; } = new();

    public static readonly Validator V = new();

    public class Validator : MudAbstractValidator<VincularUsuarioAEmpresaFormData>
    {
        public Validator()
        {
            RuleFor(x => x.Usuario)
                .Must(x => x != null && x.Id > 0)
                .WithMessage("Informe um usuário");
        }
    }
}
