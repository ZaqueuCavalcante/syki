using Syki.Back.Emails;

namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

public record SendUserRegisterEmailConfirmationTask(Guid Id) : ISykiTask;

public class SendUserRegisterEmailConfirmationTaskHandler(SykiDbContext ctx, IEmailsService emailsService) : ISykiTaskHandler<SendUserRegisterEmailConfirmationTask>
{
    public async Task Handle(SendUserRegisterEmailConfirmationTask task)
    {
        var register = await ctx.UserRegisters.AsNoTracking().FirstAsync(d => d.Id == task.Id);

        await emailsService.SendUserRegisterEmailConfirmation(register.Email, register.Id.ToString());
    }
}
