using Syki.Back.Features.Cross.LinkOldNotifications;

namespace Syki.Back.Features.Academic.CreateTeacher;

[DomainEventDescription("Professor criado")]
public record TeacherCreatedDomainEvent(Guid UserId, Guid InstitutionId) : IDomainEvent;

public class TeacherCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<TeacherCreatedDomainEvent>
{
    public async Task Handle(Guid eventId, TeacherCreatedDomainEvent evt)
    {
        await ctx.SaveTaskAsync(eventId, new LinkOldNotificationsTask(evt.UserId, evt.InstitutionId));
    }
}
