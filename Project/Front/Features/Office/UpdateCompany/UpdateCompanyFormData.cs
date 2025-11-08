using FluentValidation;
using Exato.Front.Components.Empresas;

namespace Exato.Front.Features.Office.UpdateCompany;

public class UpdateCompanyFormData
{
    public Guid ExternalId { get; set; }
    public ExatoWebOnboardStatus? OnboardStatus { get; set; }

    public static readonly Validator V = new();

    public class Validator : MudAbstractValidator<UpdateCompanyFormData>
    {
        public Validator()
        {
            RuleFor(x => x.OnboardStatus).NotEmpty().WithMessage("Informe o status");
        }
    }
}
