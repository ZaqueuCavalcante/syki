using Syki.Back.Emails;

namespace Syki.Back.CreatePendingUserRegister;

public class SendUserRegisterEmailConfirmation
{
    public string Email { get; set; }
}

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
