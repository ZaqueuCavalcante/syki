namespace Estud.Back.Features.Identity.GoogleOneTapLogin;

public class GoogleOneTapLoginIn : IApiDto<GoogleOneTapLoginIn>
{
    public string? Credential { get; set; }

    public static IEnumerable<(string Name, GoogleOneTapLoginIn Value)> GetExamples() =>
    [
        ("Google ID Token",
        new GoogleOneTapLoginIn
        {
            Credential = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9...",
        }),
    ];
}
