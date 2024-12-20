using Syki.Back.Emails;

namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

public record SendUserRegisterEmailConfirmationTask(Guid UserRegisterId) : ISykiTask;

public class SendUserRegisterEmailConfirmationTaskHandler(SykiDbContext ctx, IEmailsService emailsService) : ISykiTaskHandler<SendUserRegisterEmailConfirmationTask>
{
    public async Task Handle(SendUserRegisterEmailConfirmationTask task)
    {
        var register = await ctx.UserRegisters.AsNoTracking().FirstAsync(d => d.Id == task.UserRegisterId);

        await emailsService.SendUserRegisterEmailConfirmation(register.Email, register.Id.ToString());
    }
}
