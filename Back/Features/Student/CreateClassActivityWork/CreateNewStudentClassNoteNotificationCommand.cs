using Syki.Back.Features.Academic.CreateNotification;

namespace Syki.Back.Features.Student.CreateClassActivityWork;

[CommandDescription("Criar notificação de nota adicionada")]
public record CreateNewStudentClassNoteNotificationCommand(Guid ClassActivityId, Guid UserId) : ICommand;

public class CreateNewStudentClassNoteNotificationCommandHandler(SykiDbContext ctx) : ICommandHandler<CreateNewStudentClassNoteNotificationCommand>
{
    public async Task Handle(Guid commandId, CreateNewStudentClassNoteNotificationCommand command)
    {
        var activity = await ctx.ClassActivities
            .Where(x => x.Id == command.ClassActivityId)
            .Select(x => new { x.ClassId, x.Title })
            .FirstAsync();

        var institutionId = await ctx.Classes
            .Where(x => x.Id == activity.ClassId)
            .Select(x => x.InstitutionId)
            .FirstAsync();

        var notification = new Notification(
            institutionId,
            "Nota adicionada",
            $"Confira sua nota na atividade: {activity.Title}",
            UsersGroup.Students,
            false
        );

        ctx.AddRange(notification, new UserNotification(command.UserId, notification.Id));
    }
}
