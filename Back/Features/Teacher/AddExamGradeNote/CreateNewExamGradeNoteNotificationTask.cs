using Syki.Back.Features.Academic.CreateNotification;

namespace Syki.Back.Features.Teacher.AddExamGradeNote;

public record CreateNewExamGradeNoteNotificationTask(Guid UserId, Guid ClassId) : ISykiTask;

public class CreateNewExamGradeNoteNotificationTaskHandler(SykiDbContext ctx) : ISykiTaskHandler<CreateNewExamGradeNoteNotificationTask>
{
    public async Task Handle(CreateNewExamGradeNoteNotificationTask task)
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
