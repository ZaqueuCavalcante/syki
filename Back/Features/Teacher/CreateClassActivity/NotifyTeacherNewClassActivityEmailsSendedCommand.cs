using Syki.Back.Features.Academic.CreateNotification;

namespace Syki.Back.Features.Teacher.CreateClassActivity;

[CommandDescription("Notificar professor (emails da nova atividade)")]
public record NotifyTeacherNewClassActivityEmailsSendedCommand(Guid InstitutionId, Guid TeacherId, Guid ClassActivityId) : ICommand;

public class NotifyTeacherNewClassActivityEmailsSendedCommandHandler(SykiDbContext ctx) : ICommandHandler<NotifyTeacherNewClassActivityEmailsSendedCommand>
{
    public async Task Handle(CommandId commandId, NotifyTeacherNewClassActivityEmailsSendedCommand command)
    {
        var activity = await ctx.ClassActivities
            .Where(x => x.Id == command.ClassActivityId).Select(x => new { x.Title }).FirstAsync();

        var notification = new Notification(
            command.InstitutionId,
            "Nova atividade enviada",
            $"'{activity.Title}' enviada com sucesso!",
            UsersGroup.Teachers,
            false
        );

        ctx.AddRange(notification, new UserNotification(command.TeacherId, notification.Id));
    }
}
