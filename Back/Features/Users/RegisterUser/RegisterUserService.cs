using Estud.Back.Domain.Identity;
using Estud.Back.Domain.Institutions;

namespace Estud.Back.Features.Users.RegisterUser;

public class RegisterUserService(EstudDbContext ctx, UserManager<EstudUser> userManager) : IEstudService
{
    private class Validator : AbstractValidator<RegisterUserIn>
    {
        public Validator()
        {
            RuleFor(x => x.Email).Must(x => x.IsValidEmail()).WithError(InvalidEmail.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<RegisterUserOut, EstudError>> Register(RegisterUserIn data)
    {
        if (V.Run(data, out var error)) return error;

        var email = data.Email.ToLowerInvariant();
        var emailUsed = await ctx.Users.AnyAsync(u => u.Email == email);
        if (emailUsed) return EmailAlreadyUsed.I;

        var institution = Institution.NewForUserRegister();
        var directorRole = institution.GetDirectorRole();

        var user = new EstudUser(institution, "Seu Nome", email);
        var userRole = new EstudUserRole(institution, user, directorRole);
        var magicLink = new MagicLink(user);

        ctx.AddRange(institution, magicLink, userRole);
        ctx.AddCommand(institution, new SendFirstAccessMagicLinkEmailCommand(email, magicLink.Id), maxRetries: 1);

        await userManager.CreateAsync(user, $"Estud@{Guid.NewGuid()}");

        return new RegisterUserOut { Id = user.Id, InstitutionId = institution.Id };
    }
}
