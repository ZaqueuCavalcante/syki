using Syki.Back.Emails;
using Syki.Back.Domain.Identity;

namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

public record SendFirstAccessMagicLinkEmailCommand(string Email) : ICommand;

public class SendFirstAccessMagicLinkEmailCommandHandler(
    SykiDbContext ctx,
    IEmailsService emailService) : ICommandHandler<SendFirstAccessMagicLinkEmailCommand>
{
    public async Task Handle(Guid commandId, SendFirstAccessMagicLinkEmailCommand command)
    {
        var user = await ctx.Users.Where(x => x.Email == command.Email).Select(x => new { x.Id, x.Email }).FirstAsync();

        // Invalidate any existing magic link tokens for this user
        var existingTokens = await ctx.WebMagicLinks
            .Where(t => t.UserId == user.Id && t.UsedAt == null)
            .ToListAsync();

        foreach (var existing in existingTokens) existing.Use();

        var magicLink = new MagicLink(user.Id);
        ctx.Add(magicLink);

        await emailService.SendFirstAccessMagicLinkEmail(user.Email!, magicLink.Id.ToString());
    }
}
