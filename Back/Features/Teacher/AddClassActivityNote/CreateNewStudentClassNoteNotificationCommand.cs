using Syki.Back.Features.Academic.CreateNotification;

namespace Syki.Back.Features.Teacher.AddClassActivityNote;

[CommandDescription("Criar notificação de nota adicionada")]
public record CreateNewStudentClassNoteNotificationCommand(Guid ClassId, Guid UserId) : ICommand;

public class CreateNewStudentClassNoteNotificationCommandHandler(SykiDbContext ctx) : ICommandHandler<CreateNewStudentClassNoteNotificationCommand>
{
    public async Task Handle(Guid commandId, CreateNewStudentClassNoteNotificationCommand command)
    {
        var @class = await ctx.Classes.AsNoTracking()
            .Include(x => x.Discipline)
            .FirstAsync(x => x.Id == command.ClassId);

        var notification = new Notification(
            @class.InstitutionId,
            "Nota adicionada",
            $"Confira sua nota na disciplina: {@class.Discipline.Name}",
            UsersGroup.Students,
            false
        );

        ctx.AddRange(notification, new UserNotification(command.UserId, notification.Id));
    }
}
