using Syki.Back.Features.Cross.LinkOldNotifications;

namespace Syki.Back.Features.Academic.CreateStudent;

[DomainEventDescription("Aluno criado")]
public record StudentCreatedDomainEvent(Guid UserId, Guid InstitutionId) : IDomainEvent;

public class StudentCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<StudentCreatedDomainEvent>
{
    public async Task Handle(Guid eventId, StudentCreatedDomainEvent evt)
    {
        await ctx.SaveTasksAsync<ISykiTask>(
            eventId,
            new LinkOldNotificationsTask(evt.UserId, evt.InstitutionId),
            new SendStudentWelcomeEmailTask(evt.UserId, evt.InstitutionId)
        );
    }
}
