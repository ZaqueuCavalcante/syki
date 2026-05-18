using Syki.Back.Domain.Identity;
using Syki.Back.Domain.Institutions;

namespace Syki.Back.Features.Users.RegisterUser;

public class RegisterUserService(SykiDbContext ctx, UserManager<SykiUser> userManager) : ISykiService
{
    private class Validator : AbstractValidator<RegisterUserIn>
    {
        public Validator()
        {
            RuleFor(x => x.Email).Must(x => x.IsValidEmail()).WithError(InvalidEmail.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<RegisterUserOut, SykiError>> Register(RegisterUserIn data)
    {
        if (V.Run(data, out var error)) return error;

        var email = data.Email.ToLowerInvariant();
        var emailUsed = await ctx.Users.AnyAsync(u => u.Email == email);
        if (emailUsed) return EmailAlreadyUsed.I;

        var institution = Institution.NewForUserRegister();
        var user = new SykiUser(institution, email, email);
        var magicLink = new MagicLink(user);

        ctx.AddRange(institution, magicLink);
        var command = ctx.AddCommand(institution, new SendFirstAccessMagicLinkEmailCommand(email, magicLink.Id), maxRetries: 1);

        await userManager.CreateAsync(user, $"Syki@{Guid.NewGuid()}");

        return new RegisterUserOut { Id = user.Id, InstitutionId = institution.Id };
    }
}
