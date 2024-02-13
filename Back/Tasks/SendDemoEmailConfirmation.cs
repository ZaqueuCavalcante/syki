using Syki.Back.Database;
using Syki.Back.Services;
using Syki.Back.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Syki.Back.Tasks;

public class SendDemoEmailConfirmation
{
    public string Email { get; set; }
}

public class SendDemoEmailConfirmationHandler : ISykiTaskHandler<SendDemoEmailConfirmation>
{
    private readonly SykiDbContext _ctx;
    private readonly IEmailsService _emailsService;
    public SendDemoEmailConfirmationHandler(
        SykiDbContext ctx,
        IEmailsService emailsService
    ) {
        _ctx = ctx;
        _emailsService = emailsService;
    }

    public async Task Handle(SendDemoEmailConfirmation task)
    {
        var demo = await _ctx.Demos.FirstOrDefaultAsync(d => d.Email == task.Email);
        if (demo == null)
            Throw.DE1103.Now();

        var message = $"https://localhost:6001/demo-setup?token={demo.Id}";

        _emailsService.Send(demo.Email, message);
    }
}
