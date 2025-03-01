using Syki.Back.Features.Academic.CreateNotification;

namespace Syki.Back.Features.Teacher.AddExamGradeNote;

[CommandDescription("Criar notificação de nota adicionada")]
public record CreateNewExamGradeNoteNotificationCommand(Guid ClassId, Guid UserId) : ICommand;

public class CreateNewExamGradeNoteNotificationCommandHandler(SykiDbContext ctx) : ICommandHandler<CreateNewExamGradeNoteNotificationCommand>
{
    public async Task Handle(CreateNewExamGradeNoteNotificationCommand command)
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

        await ctx.SaveChangesAsync();
    }
}
