using Syki.Back.Features.Academic.CreateNotification;

namespace Syki.Back.Features.Teacher.CreateClassActivity;

[CommandDescription("Criar notificação de nova atividade")]
public record CreateNewClassActivityNotificationCommand(Guid ClassActivityId) : ICommand;

public class CreateNewClassActivityNotificationCommandHandler(SykiDbContext ctx) : ICommandHandler<CreateNewClassActivityNotificationCommand>
{
    public async Task Handle(Guid commandId, CreateNewClassActivityNotificationCommand command)
    {
        var activity = await ctx.ClassActivities
            .Where(x => x.Id == command.ClassActivityId).Select(x => new { x.ClassId, x.Title }).FirstAsync();

        var @class = await ctx.Classes.AsNoTracking()
            .Include(x => x.Discipline)
            .FirstAsync(x => x.Id == activity.ClassId);

        var students = await ctx.ClassesStudents
            .Where(x => x.ClassId == @class.Id)
            .Select(x => new { Id = x.SykiStudentId })
            .ToListAsync();

        var notification = new Notification(
            @class.InstitutionId,
            "Nova atividade",
            $"{@class.Discipline.Name}: {activity.Title}",
            UsersGroup.Students,
            false
        );
        ctx.Add(notification);

        var batch = CommandBatch.New(@class.InstitutionId, CommandBatchType.SendNewClassActivityEmailCommands);
        ctx.Add(batch);
        foreach (var student in students)
        {
            ctx.Add(new UserNotification(student.Id, notification.Id));
            ctx.AddCommand(@class.InstitutionId, new SendNewClassActivityEmailCommand(student.Id, notification.Id), parentId: commandId, batchId: batch.Id);
        }

        var nexCommand = ctx.AddCommand(@class.InstitutionId, new NotifyTeacherNewClassActivityEmailsSendedCommand(@class.InstitutionId, @class.TeacherId, command.ClassActivityId));
        batch.ContinueWith(nexCommand);
    }
}
