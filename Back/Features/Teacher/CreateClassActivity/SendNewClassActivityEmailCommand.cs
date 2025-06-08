using Syki.Back.Emails;

namespace Syki.Back.Features.Teacher.CreateClassActivity;

[CommandDescription("Enviar email de nova atividade")]
public record SendNewClassActivityEmailCommand(Guid UserId, Guid NotificationId) : ICommand;

public class SendNewClassActivityEmailCommandHandler(SykiDbContext ctx, IEmailsService emailsService) : ICommandHandler<SendNewClassActivityEmailCommand>
{
    public async Task Handle(CommandId commandId, SendNewClassActivityEmailCommand command)
    {
        var email = await ctx.Users.Where(x => x.Id == command.UserId).Select(x => x.Email).FirstAsync();
        var notification = await ctx.Notifications.Where(d => d.Id == command.NotificationId).Select(x => x.Description).FirstAsync();

        await emailsService.SendNewClassActivityEmail(email, notification);
    }
}
