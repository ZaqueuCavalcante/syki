using Syki.Back.Emails;
using Syki.Back.Domain.Identity;

namespace Syki.Back.Features.Users.RegisterUser;

public record SendFirstAccessMagicLinkEmailCommand(string Email) : ICommand;

public class SendFirstAccessMagicLinkEmailCommandHandler(
    SykiDbContext ctx,
    IEmailsService emailService) : ICommandHandler<SendFirstAccessMagicLinkEmailCommand>
{
    public async Task Handle(int commandId, SendFirstAccessMagicLinkEmailCommand command)
    {
        var user = await ctx.Users.Where(x => x.Email == command.Email).Select(x => new { x.Id, x.Email }).FirstAsync();

        var magicLink = new MagicLink(user.Id);
        ctx.Add(magicLink);

        await emailService.SendFirstAccessMagicLinkEmail(user.Email!, magicLink.Id.ToString());
    }
}
