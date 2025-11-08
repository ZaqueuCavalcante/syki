using Exato.Shared.Enums;
using Exato.Shared.Extensions;

namespace Exato.Web.Domain;

public class Company
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string CompanyUid { get; set; }

    public bool Active { get; set; }

    public string? PathSocialContract { get; set; }

    public string Cnpj { get; set; }

    /// <summary>
    /// Ver <see cref="ExatoWebOnboardStatus"/>
    /// </summary>
    public int OnboardStatus { get; set; }

    public DateTime OnboardDate { get; set; }

    /// <summary>
    /// Ver <see cref="CompanyPaymentMode"/>
    /// </summary>
    public int PaymentMode { get; set; }

    public int? AddressCommercialId { get; set; }

    public int? AddressFiscalId { get; set; }

    public Guid? ExternalId { get; set; }

    public bool? CameFromRegisterPostPaid { get; set; }

    public int? TransactionReusePeriodMonths { get; set; }

    public Company() { }

    public Company(
        Guid externalId,
        string cnpj,
        string name,
        CompanyPaymentMode paymentMode)
    {
        Cnpj = cnpj;
        Name = name;
        Active = true;
        PaymentMode = paymentMode.ToShort();
        OnboardStatus = ExatoWebOnboardStatus.Completed.ToInt();
        OnboardDate = DateTime.Now;
        ExternalId = externalId;
        CompanyUid = externalId.ToString();
    }

    public void EditarCadastro(
        bool active,
        string cnpj,
        string name)
    {
        Active = active;
        Cnpj = cnpj;
        Name = name;
    }

    public void EditarMetodoDePagamento(CompanyPaymentMode paymentMode)
    {
        PaymentMode = paymentMode.ToShort();
    }

    public void UpdateOnboardStatus(ExatoWebOnboardStatus onboardStatus)
    {
        OnboardStatus = onboardStatus.ToInt();

        if (onboardStatus == ExatoWebOnboardStatus.Completed)
        {
            Active = true;
        }
        else
        {
            Active = false;
        }
    }
}
