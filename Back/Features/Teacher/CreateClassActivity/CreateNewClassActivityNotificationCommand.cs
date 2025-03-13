using Syki.Back.Features.Academic.CreateNotification;

namespace Syki.Back.Features.Teacher.CreateClassActivity;

[CommandDescription("Criar notificação de nova atividade")]
public record CreateNewClassActivityNotificationCommand(Guid ClassId) : ICommand;

public class CreateNewClassActivityNotificationCommandHandler(SykiDbContext ctx) : ICommandHandler<CreateNewClassActivityNotificationCommand>
{
    public async Task Handle(Guid commandId, CreateNewClassActivityNotificationCommand command)
    {
        var @class = await ctx.Classes.AsNoTracking()
            .Include(x => x.Discipline)
            .FirstAsync(x => x.Id == command.ClassId);

        var students = await ctx.ClassesStudents
            .Where(x => x.ClassId == command.ClassId)
            .Select(x => new { Id = x.SykiStudentId })
            .ToListAsync();

        var notification = new Notification(
            @class.InstitutionId,
            "Nova atividade",
            $"Confira a nova atividade da disciplina: {@class.Discipline.Name}",
            UsersGroup.Students,
            false
        );
        ctx.Add(notification);

        var batch = CommandBatch.New(@class.InstitutionId);
        ctx.Add(batch);
        foreach (var student in students)
        {
            ctx.Add(new UserNotification(student.Id, notification.Id));
            ctx.AddCommand(@class.InstitutionId, new SendNewClassActivityEmailCommand(student.Id, notification.Id), parentId: commandId, batchId: batch.Id);
        }
    }
}
