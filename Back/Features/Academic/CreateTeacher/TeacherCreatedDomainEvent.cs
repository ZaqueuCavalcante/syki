using Syki.Back.Features.Cross.LinkOldNotifications;

namespace Syki.Back.Features.Academic.CreateTeacher;

[DomainEventDescription("Professor criado")]
public record TeacherCreatedDomainEvent(Guid UserId, Guid InstitutionId) : IDomainEvent;

public class TeacherCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<TeacherCreatedDomainEvent>
{
    public async Task Handle(Guid eventId, Guid institutionId, TeacherCreatedDomainEvent evt)
    {
        await ctx.SaveCommandsAsync(eventId, institutionId, new LinkOldNotificationsCommand(evt.InstitutionId, evt.UserId));
    }
}
