using Syki.Back.Auth.Managers;

namespace Syki.Back.Features.Identity.UpdateSsoConfiguration;

public class UpdateSsoConfigurationService(SykiDbContext ctx, SsoEncryptionManager encryption, SsoSchemeManager ssoSchemeManager) : ISykiService
{
    private class Validator : AbstractValidator<UpdateSsoConfigurationIn>
    {
        public Validator()
        {
            RuleFor(x => x.ProviderType).IsInEnum().WithError(InvalidSsoProviderType.I);
            RuleFor(x => x.Authority).NotEmpty().WithError(InvalidSsoAuthority.I);
            RuleFor(x => x.ClientId).NotEmpty().WithError(InvalidSsoClientId.I);
            RuleFor(x => x.ClientId).MinimumLength(5).WithError(InvalidSsoClientId.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<UpdateSsoConfigurationOut, SykiError>> Update(Guid id, UpdateSsoConfigurationIn data)
    {
        if (V.Run(data, out var error)) return error;

        var authorityError = data.Authority.ValidateSsoAuthority();
        if (authorityError != null) return authorityError;

        var config = await ctx.WebSsoConfigurations
            .Where(x => x.PublicId == id && x.InstitutionId == ctx.RequestUser.InstitutionId)
            .FirstOrDefaultAsync();

        if (config == null) return SsoConfigurationNotFound.I;

        var clientSecret = string.IsNullOrEmpty(data.ClientSecret)
            ? config.ClientSecret
            : encryption.Encrypt(data.ClientSecret);

        config.Update(
            data.ProviderType,
            data.Authority.TrimEnd('/'),
            data.ClientId.Trim(),
            clientSecret,
            data.IsActive,
            data.RequireSso);

        await ctx.SaveChangesAsync();

        ssoSchemeManager.RegisterScheme(config);

        return new UpdateSsoConfigurationOut { Id = config.PublicId };
    }
}
