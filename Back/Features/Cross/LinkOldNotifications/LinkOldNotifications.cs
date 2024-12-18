namespace Syki.Back.Features.Cross.LinkOldNotifications;

public class LinkOldNotifications : ISykiTask
{
    public Guid UserId { get; set; }
    public Guid InstitutionId { get; set; }
}
