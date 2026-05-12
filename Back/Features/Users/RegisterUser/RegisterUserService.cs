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
        ctx.Add(institution);

        ctx.AddCommand(institution.Id, new SendFirstAccessMagicLinkEmailCommand(email), maxRetries: 1);

        var user = new SykiUser(institution.Id, email, email);
        await userManager.CreateAsync(user, $"Syki@{Guid.NewGuid()}");

        return new RegisterUserOut { Id = user.Id };
    }
}
