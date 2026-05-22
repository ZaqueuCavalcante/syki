using Syki.Back.Auth.Managers;
using Syki.Back.Domain.Identity;

namespace Syki.Back.Features.Identity.CreateSsoConfiguration;

public class CreateSsoConfigurationService(SykiDbContext ctx, SsoEncryptionManager encryption, SsoSchemeManager ssoSchemeManager) : ISykiService
{
    private class Validator : AbstractValidator<CreateSsoConfigurationIn>
    {
        public Validator()
        {
            RuleFor(x => x.ProviderType).IsInEnum().WithError(InvalidSsoProviderType.I);

            RuleFor(x => x.Authority).NotEmpty().WithError(InvalidSsoAuthority.I);

            RuleFor(x => x.ClientId).NotEmpty().WithError(InvalidSsoClientId.I);
            RuleFor(x => x.ClientId).MinimumLength(5).WithError(InvalidSsoClientId.I);

            RuleFor(x => x.ClientSecret).NotEmpty().WithError(InvalidSsoClientSecret.I);
            RuleFor(x => x.ClientSecret).MinimumLength(10).WithError(InvalidSsoClientSecret.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CreateSsoConfigurationOut, SykiError>> Create(CreateSsoConfigurationIn data)
    {
        if (V.Run(data, out var error)) return error;

        var authorityError = data.Authority.ValidateSsoAuthority();
        if (authorityError != null) return authorityError;

        var userEmail = await ctx.Users.Where(x => x.Id == ctx.RequestUser.Id).Select(x => x.Email).FirstAsync();

        var domain = userEmail!.Split('@').Last().NormalizeSsoDomain();
        if (domain == null) return InvalidSsoAllowedDomains.I;

        var domainExists = await ctx.WebSsoAllowedDomains.AnyAsync(d => d.Domain == domain);
        if (domainExists) return SsoDomainAlreadyConfigured.I;

        var config = new SsoConfiguration(
            ctx.RequestUser.InstitutionId,
            data.ProviderType,
            data.Authority.TrimEnd('/'),
            data.ClientId.Trim(),
            encryption.Encrypt(data.ClientSecret),
            [domain]);

        await ctx.SaveChangesAsync(config);

        ssoSchemeManager.RegisterScheme(config);

        return new CreateSsoConfigurationOut { Id = config.PublicId };
    }
}
