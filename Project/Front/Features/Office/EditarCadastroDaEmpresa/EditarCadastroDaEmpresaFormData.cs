using FluentValidation;
using Exato.Front.Components.Empresas;

namespace Exato.Front.Features.Office.EditarCadastroDaEmpresa;

public class EditarCadastroDaEmpresaFormData
{
    public int Id { get; set; }
    public bool Ativa { get; set; }
    public string Nome { get; set; }
    public string CNPJ { get; set; }
    public string RazaoSocial { get; set; }
    public string? NomeFantasia { get; set; }
    public string? Slug { get; set; }
    public string? SalesContact { get; set; }

    public MatrizSelectData Matriz { get; set; } = new();

    public static readonly Validator V = new();

    public class Validator : MudAbstractValidator<EditarCadastroDaEmpresaFormData>
    {
        public Validator()
        {
            RuleFor(x => x.Nome).NotEmpty().WithMessage("Informe o nome");

            RuleFor(x => x.RazaoSocial).NotEmpty().WithMessage("Informe a razão social");

            RuleFor(x => x.CNPJ).Must(x => x.IsValidCnpj()).WithMessage("CNPJ inválido");

            When(x => x.Matriz != null && x.Matriz.Id != 0, () =>
            {
                RuleFor(x => x.SalesContact).Must(x => x.IsEmpty()).WithMessage("Apenas matrizes podem possuir vendedor responsável");
            });
        }
    }
}
