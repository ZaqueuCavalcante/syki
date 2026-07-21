using Estud.Back.Domain.Notifications;

namespace Estud.Back.Features.Teachers.CreateClassActivity;

[CommandDescription("Criar notificação de nova atividade")]
public record CreateNewClassActivityNotificationCommand(int ClassActivityId) : ICommand;

public class CreateNewClassActivityNotificationCommandHandler(EstudDbContext ctx) : ICommandHandler<CreateNewClassActivityNotificationCommand>
{
    public async Task Handle(int commandId, CreateNewClassActivityNotificationCommand command)
    {
        var activity = await ctx.ClassActivities.Where(x => x.Id == command.ClassActivityId)
            .Select(x => new { x.ClassId, x.Title }).FirstAsync();

        var @class = await ctx.Classes.AsNoTracking()
            .Include(x => x.Discipline)
            .FirstAsync(x => x.Id == activity.ClassId);

        var students = await ctx.ClassStudents
            .Where(x => x.ClassId == @class.Id)
            .Select(x => new { Id = x.StudentId })
            .ToListAsync();

        var notification = new Notification(
            @class.InstitutionId,
            NotificationType.NewClassActivity,
            "Nova atividade",
            $"{@class.Discipline.Name}: {activity.Title}"
        );
        ctx.Add(notification);

        foreach (var student in students)
        {
            ctx.Add(new UserNotification(student.Id, notification));
        }
    }
}
