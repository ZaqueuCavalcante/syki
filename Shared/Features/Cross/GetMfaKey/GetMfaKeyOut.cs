namespace Syki.Shared;

public class GetMfaKeyOut
{
    /// <summary>
    /// Chave MFA do usuário.
    /// </summary>
    public string Key { get; set; }

    public static IEnumerable<(string, GetMfaKeyOut)> GetExamples() =>
    [
        ("Exemplo", new() { Key = "COZF2TE2BEWGHEB77A5THFYHPBC2KHPM" }),
    ];
}
