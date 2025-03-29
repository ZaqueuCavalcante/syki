using Syki.Back.Features.Cross.LinkOldNotifications;

namespace Syki.Back.Features.Academic.CreateTeacher;

[DomainEvent(nameof(SykiTeacher), "Professor criado")]
public record TeacherCreatedDomainEvent(Guid UserId, Guid InstitutionId) : IDomainEvent;

public class TeacherCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<TeacherCreatedDomainEvent>
{
    public async Task Handle(Guid institutionId, Guid eventId, TeacherCreatedDomainEvent evt)
    {
        ctx.AddCommand(institutionId, new LinkOldNotificationsCommand(evt.InstitutionId, evt.UserId), eventId: eventId);
        await Task.CompletedTask;
    }
}
