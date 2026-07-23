using Estud.Back.Domain.Classes;

namespace Estud.Back.Features.Teachers.CreateClassActivity;

public class ClassActivityCreatedHandler(EstudDbContext ctx) : IDomainEventHandler<ClassActivityCreated>
{
    public async Task Handle(int institutionId, int eventId, ClassActivityCreated evt)
    {
        var activityId = await ctx.ClassActivities.AsNoTracking()
            .Where(x => x.Uid == evt.Uid)
            .Select(x => x.Id)
            .FirstAsync();

        ctx.AddCommand(institutionId, new CreateNewClassActivityNotificationCommand(activityId));
    }
}
