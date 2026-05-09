using Syki.Back.Features.Cross.SignIn;

namespace Syki.Back.Features.Identity.MagicLinkLogin;

public class MagicLinkLoginService(SykiDbContext ctx, SignInService signInService) : IAcademicService
{
    private class Validator : AbstractValidator<MagicLinkLoginIn>
    {
        public Validator()
        {
            RuleFor(x => x.Token).NotEmpty().WithError(InvalidMagicLink.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<MagicLinkLoginOut, SykiError>> Login(MagicLinkLoginIn data)
    {
        if (V.Run(data, out var error)) return error;

        if (!Guid.TryParse(data.Token, out var tokenId)) return InvalidMagicLink.I;

        var magicLink = await ctx.WebMagicLinks.FirstOrDefaultAsync(t => t.Id == tokenId);
        if (magicLink == null) return InvalidMagicLink.I;

        if (magicLink.IsUsed()) return InvalidMagicLink.I;

        if (magicLink.IsExpired()) return InvalidMagicLink.I;
    
        magicLink.Use();

        var user = await ctx.Users.FirstAsync(x => x.Id == magicLink.UserId);
        user.ConfirmEmail();

        var result = await signInService.SignIn(user.Email);

        await ctx.SaveChangesAsync();

        return result.ToMagicLinkLoginOut();
    }
}
