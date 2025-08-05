namespace Syki.Shared;

public class SetupMfaIn
{
    public string Token { get; set; }

    public static IEnumerable<(string, SetupMfaIn)> GetExamples() =>
    [
        ("Exemplo", new() { Token = "843972" }),
    ];
}
