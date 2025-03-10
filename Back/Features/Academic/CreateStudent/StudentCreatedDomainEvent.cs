using Syki.Back.Features.Cross.LinkOldNotifications;

namespace Syki.Back.Features.Academic.CreateStudent;

[DomainEventDescription("Aluno criado")]
public record StudentCreatedDomainEvent(Guid UserId, Guid InstitutionId) : IDomainEvent;

public class StudentCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<StudentCreatedDomainEvent>
{
    public async Task Handle(Guid institutionId, Guid eventId, StudentCreatedDomainEvent evt)
    {
        ctx.AddCommand(institutionId, new LinkOldNotificationsCommand(evt.InstitutionId, evt.UserId), eventId: eventId);
        ctx.AddCommand(institutionId, new SendStudentWelcomeEmailCommand(evt.InstitutionId, evt.UserId), eventId: eventId);
        await Task.CompletedTask;
    }
}
