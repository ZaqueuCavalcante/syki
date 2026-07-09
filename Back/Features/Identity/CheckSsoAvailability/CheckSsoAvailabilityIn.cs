namespace Estud.Back.Features.Identity.CheckSsoAvailability;

public class CheckSsoAvailabilityIn : IApiDto<CheckSsoAvailabilityIn>
{
    public string Email { get; set; }

    public static IEnumerable<(string Name, CheckSsoAvailabilityIn Value)> GetExamples() =>
    [
        ("Exemplo", new CheckSsoAvailabilityIn { Email = "usuario@empresa.com.br" }),
    ];
}
