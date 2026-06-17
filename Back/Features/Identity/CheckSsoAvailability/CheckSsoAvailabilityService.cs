using Dapper;

namespace Syki.Back.Features.Identity.CheckSsoAvailability;

public class CheckSsoAvailabilityService(SykiDbContext ctx) : ISykiService
{
    private class Validator : AbstractValidator<CheckSsoAvailabilityIn>
    {
        public Validator()
        {
            RuleFor(x => x.Email).NotEmpty().WithError(InvalidEmail.I);
            RuleFor(x => x.Email).Must(x => x.IsValidEmail()).WithError(InvalidEmail.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CheckSsoAvailabilityOut, SykiError>> Check(CheckSsoAvailabilityIn data)
    {
        if (V.Run(data, out var error)) return error;

        var domain = ExtractDomain(data.Email);
        if (domain == null) return new CheckSsoAvailabilityOut { SsoEnabled = false };

        const string sql = @"
            SELECT
                c.require_sso,
                c.provider_type
            FROM
                syki.sso_allowed_domains d
            INNER JOIN
                syki.sso_configurations c ON c.id = d.sso_configuration_id
            WHERE
                d.domain = @Domain
                    AND
                c.is_active = true
            LIMIT 1
        ";

        var config = await ctx.Database.GetDbConnection().QueryFirstOrDefaultAsync<SsoConfigDto>(sql, new { Domain = domain });
        if (config == null) return new CheckSsoAvailabilityOut { SsoEnabled = false };

        return new CheckSsoAvailabilityOut
        {
            SsoEnabled = true,
            SsoRequired = config.RequireSso,
            ProviderType = config.ProviderType,
        };
    }

    private static string? ExtractDomain(string email)
    {
        if (email.IsEmpty()) return null;

        var parts = email.Split('@');
        if (parts.Length != 2) return null;

        return parts[1].Trim().ToLowerInvariant();
    }
}
