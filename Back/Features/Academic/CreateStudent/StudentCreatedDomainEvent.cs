using Syki.Back.Features.Cross.LinkOldNotifications;

namespace Syki.Back.Features.Academic.CreateStudent;

public record StudentCreatedDomainEvent(Guid UserId, Guid InstitutionId) : IDomainEvent;

public class StudentCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<StudentCreatedDomainEvent>
{
    public async Task Handle(StudentCreatedDomainEvent evt)
    {
        await ctx.SaveTaskAsync(new LinkOldNotificationsTask(evt.UserId, evt.InstitutionId));
    }
}
