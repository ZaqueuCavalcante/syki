using Syki.Back.Features.Cross.LinkOldNotifications;

namespace Syki.Back.Features.Academic.CreateTeacher;

public record TeacherCreatedDomainEvent(Guid UserId, Guid InstitutionId) : IDomainEvent;

public class TeacherCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<TeacherCreatedDomainEvent>
{
    public async Task Handle(TeacherCreatedDomainEvent evt)
    {
        await ctx.SaveTaskAsync(new LinkOldNotificationsTask(evt.UserId, evt.InstitutionId));
    }
}
