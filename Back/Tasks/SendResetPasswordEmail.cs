using Syki.Back.Database;
using Syki.Back.Services;
using Syki.Back.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Tasks;

public class SendResetPasswordEmail
{
    public Guid UserId { get; set; }
}

public class SendResetPasswordEmailHandler : ISykiTaskHandler<SendResetPasswordEmail>
{
    private readonly SykiDbContext _ctx;
    private readonly IEmailsService _emailsService;
    public SendResetPasswordEmailHandler(
        SykiDbContext ctx,
        IEmailsService emailsService
    ) {
        _ctx = ctx;
        _emailsService = emailsService;
    }

    public async Task Handle(SendResetPasswordEmail task)
    {
        var user = await _ctx.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == task.UserId);
        if (user == null)
            Throw.DE019.Now();

        if (user.Email.EndsWith("@syki.demo.com") || user.Name == "DEMO")
            return;

        var reset = await _ctx.ResetPasswords
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefaultAsync(r => r.UserId == user.Id && r.UsedAt == null);

        await _emailsService.SendResetPasswordEmail(user.Email, reset.Id.ToString());
    }
}
