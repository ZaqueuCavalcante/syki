using FluentValidation;

namespace Exato.Front.Features.Office.EditarConsultasDaEmpresa;

public class EditarConsultasDaEmpresaFormData
{
    public int Id { get; set; }
    public bool HighPerformance { get; set; }
    public bool BlockSensitiveDataInQueryString { get; set; }
    public DataAccessLevel DataAccessLevel { get; set; } = DataAccessLevel.DadosDeCadastroCompleto;
    public int? TransLimitPerWeek { get; set; }

    public bool GerarPdfConsultas { get; set; }
    public bool HabilitarConsultasPorEmail { get; set; }

    public bool ReceitaCpfUseSerproAsMainSource { get; set; }
    public bool ReceitaCpfShouldReturnMinor18AgeData { get; set; }

    public static readonly Validator V = new();

    public class Validator : MudAbstractValidator<EditarConsultasDaEmpresaFormData>
    {
        public Validator()
        {
            RuleFor(x => x.TransLimitPerWeek)
                .Must(x => x > 0)
                .When(x => x.TransLimitPerWeek != null)
                .WithMessage("Informe um limite semanal maior ou igual a zero");
        }
    }
}
