namespace Exato.Back.Features.Web.AccountManagementCompany;

public class GetCompanyUserResponse
{
    public Guid Uid { get; set; }
    public Guid ExternalId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? LastAccessDate { get; set; }
    public string[] Claims { get; set; } = [];
    public List<GetCompanyUserCompanyResponse> Companies { get; set; } = [];
}

public class GetCompanyUserCompanyResponse
{
    public string ExternalId { get; set; }
    public string Name { get; set; }
}
