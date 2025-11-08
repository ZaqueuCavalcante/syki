namespace Exato.Web.Domain;

public class ActivitiesLog
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int? CompanyId { get; set; }

    public DateTime EventDate { get; set; }

    public string? IpAddress { get; set; }

    public string? Description { get; set; }

    public int SystemDomainId { get; set; }
}
