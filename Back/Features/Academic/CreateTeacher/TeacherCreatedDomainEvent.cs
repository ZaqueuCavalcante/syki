using Syki.Back.Features.Cross.LinkOldNotifications;

namespace Syki.Back.Features.Academic.CreateTeacher;

[DomainEventDescription("Professor criado")]
public record TeacherCreatedDomainEvent(Guid UserId, Guid InstitutionId) : IDomainEvent;

public class TeacherCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<TeacherCreatedDomainEvent>
{
    public async Task Handle(Guid institutionId, Guid eventId, TeacherCreatedDomainEvent evt)
    {
        await ctx.SaveCommandsAsync(institutionId, eventId, new LinkOldNotificationsCommand(evt.InstitutionId, evt.UserId));
    }
}
