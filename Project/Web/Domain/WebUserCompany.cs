namespace Exato.Web.Domain;

public class WebUserCompany
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int? CompanyId { get; set; }

    public int UserRole { get; set; }

    public string? UserExternalId { get; set; }

    public string? OrganizationExternalId { get; set; }

    public string IndicationCode { get; set; }

    public string? Token { get; set; }

    public bool? Main { get; set; }

    public string? PaymentProviderId { get; set; }

    public WebUserCompany() { }

    public WebUserCompany(
        int userId,
        Guid userExternalId,
        Guid organizationExternalId,
        string token,
        int? companyId = null)
    {
        UserId = userId;
        UserExternalId = userExternalId.ToString();
        OrganizationExternalId = organizationExternalId.ToString();
        Token = token;
        UserRole = 1;
        CompanyId = companyId == 0 ? null : companyId;
        IndicationCode = Guid.NewGuid().ToString("N")[..10];
    }
}
