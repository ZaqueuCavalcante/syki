using Syki.Back.Features.Teacher.AddExamGradeNote;
using Syki.Back.Features.Academic.CreateNotification;

namespace Syki.Daemon.Tasks;

public class CreateNewExamGradeNoteNotificationHandler(SykiDbContext ctx) : ISykiTaskHandler<CreateNewExamGradeNoteNotification>
{
    public async Task Handle(CreateNewExamGradeNoteNotification task)
    {
        var @class = await ctx.Classes.AsNoTracking()
            .Include(x => x.Discipline)
            .FirstAsync(x => x.Id == task.ClassId);

        var notification = new Notification(
            @class.InstitutionId,
            "Nota adicionada",
            $"Confira sua nota na disciplina: {@class.Discipline.Name}",
            UsersGroup.Students,
            false
        );

        ctx.AddRange(notification, new UserNotification(task.UserId, notification.Id));

        await ctx.SaveChangesAsync();
    }
}
