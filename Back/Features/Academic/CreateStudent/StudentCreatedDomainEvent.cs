using Syki.Back.Features.Cross.LinkOldNotifications;

namespace Syki.Back.Features.Academic.CreateStudent;

[DomainEventDescription("Aluno criado")]
public record StudentCreatedDomainEvent(Guid UserId, Guid InstitutionId) : IDomainEvent;

public class StudentCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<StudentCreatedDomainEvent>
{
    public async Task Handle(Guid eventId, Guid institutionId, StudentCreatedDomainEvent evt)
    {
        await ctx.SaveCommandsAsync(
            eventId,
            institutionId,
            new LinkOldNotificationsCommand(evt.InstitutionId, evt.UserId),
            new SendStudentWelcomeEmailCommand(evt.InstitutionId, evt.UserId)
        );
    }
}
