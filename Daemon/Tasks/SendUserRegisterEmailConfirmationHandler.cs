using Microsoft.EntityFrameworkCore;
using Syki.Back.CreatePendingUserRegister;
using Syki.Back.Database;
using Syki.Back.Exceptions;
using Syki.Back.Tasks;
using Syki.Daemon.Emails;

namespace Syki.Daemon.Tasks;

public class SendUserRegisterEmailConfirmationHandler(SykiDbContext ctx, IEmailsService emailsService)
    : ISykiTaskHandler<SendUserRegisterEmailConfirmation>
{
    public async Task Handle(SendUserRegisterEmailConfirmation task)
    {
        var register = await ctx.UserRegisters.FirstOrDefaultAsync(d => d.Email == task.Email);
        if (register == null)
            Throw.DE024.Now();

        await emailsService.SendUserRegisterEmailConfirmation(register.Email, register.Id.ToString());
    }
}
