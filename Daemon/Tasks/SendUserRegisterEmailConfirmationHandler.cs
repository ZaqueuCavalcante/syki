using Syki.Daemon.Emails;
using Syki.Back.Features.Cross.CreatePendingUserRegister;

namespace Syki.Daemon.Tasks;

public class SendUserRegisterEmailConfirmationHandler(SykiDbContext ctx, IEmailsService emailsService) : ISykiTaskHandler<SendUserRegisterEmailConfirmation>
{
    public async Task Handle(SendUserRegisterEmailConfirmation task)
    {
        var register = await ctx.UserRegisters.FirstAsync(d => d.Email == task.Email);

        await emailsService.SendUserRegisterEmailConfirmation(register.Email, register.Id.ToString());
    }
}
