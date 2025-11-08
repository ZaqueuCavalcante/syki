namespace Exato.Shared.Features.Cross.TwoFactorAuthenticationLogin;

public class TwoFactorAuthenticationLoginOut : IApiDto<TwoFactorAuthenticationLoginOut>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public List<int> Features { get; set; } = [];

    public static IEnumerable<(string, TwoFactorAuthenticationLoginOut)> GetExamples() =>
    [
        ("Exemplo",
        new TwoFactorAuthenticationLoginOut
        {
            Id = Guid.NewGuid(),
            Name = "User",
            Email = "user@exato.com",
            Role = "OrgRecruiter",
            Features = [1, 2, 3],
        }),
    ];
}
