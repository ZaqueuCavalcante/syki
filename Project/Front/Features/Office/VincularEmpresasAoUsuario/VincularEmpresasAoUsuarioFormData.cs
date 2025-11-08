using FluentValidation;
using Exato.Front.Components.Usuarios;

namespace Exato.Front.Features.Office.VincularEmpresasAoUsuario;

public class VincularEmpresasAoUsuarioFormData
{
    public PotencialEmpresaDoUsuarioSelectData Empresa { get; set; } = new();
    public List<PotencialEmpresaDoUsuarioSelectData> Empresas { get; set; } = [];

    public List<int> GetIds() => Empresas.ConvertAll(x => x.Id);

    public static readonly Validator V = new();

    public class Validator : MudAbstractValidator<VincularEmpresasAoUsuarioFormData>
    {
        public Validator()
        {
            RuleFor(x => x.Empresas)
                .Must(x => x != null && x.Count > 0)
                .WithMessage("Informe ao menos uma empresa");

            When(x => x.Empresas != null && x.Empresas.Count > 0, () =>
            {
                RuleFor(x => x.Empresas)
                    .Must(x => x.Select(y => y.Id).IsAllDistinct())
                    .WithMessage("Empresas duplicadas");
            });
        }
    }
}
