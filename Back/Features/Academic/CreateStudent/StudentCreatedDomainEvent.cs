using Syki.Back.Features.Cross.LinkOldNotifications;

namespace Syki.Back.Features.Academic.CreateStudent;

[DomainEventDescription("Aluno criado")]
public record StudentCreatedDomainEvent(Guid UserId, Guid InstitutionId) : IDomainEvent;

public class StudentCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<StudentCreatedDomainEvent>
{
    public async Task Handle(Guid institutionId, Guid eventId, StudentCreatedDomainEvent evt)
    {
        await ctx.SaveCommandsAsync(
            institutionId,
            eventId,
            new LinkOldNotificationsCommand(evt.InstitutionId, evt.UserId),
            new SendStudentWelcomeEmailCommand(evt.InstitutionId, evt.UserId)
        );
    }
}
