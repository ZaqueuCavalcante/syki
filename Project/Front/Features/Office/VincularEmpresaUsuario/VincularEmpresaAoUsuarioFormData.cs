using FluentValidation;
using Exato.Front.Components.Usuarios;

namespace Exato.Front.Features.Office.VincularEmpresaUsuario;

public class VincularEmpresaUsuarioFormData
{
    public PotencialEmpresaDoUsuarioSelectData Empresa { get; set; } = new();

    public static readonly Validator V = new();

    public class Validator : MudAbstractValidator<VincularEmpresaUsuarioFormData>
    {
        public Validator()
        {
            RuleFor(x => x.Empresa)
                .Must(x => x != null && x.Id > 0)
                .WithMessage("Informe uma empresa");
        }
    }
}
