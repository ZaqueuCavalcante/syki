namespace Syki.Shared;

public class GetMfaKeyOut
{
    /// <summary>
    /// Chave MFA do usuário.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// QrCode em Base64 contendo a chave MFA do usuário.
    /// </summary>
    public string QrCodeBase64 { get; set; }

    public static IEnumerable<(string, GetMfaKeyOut)> GetExamples() =>
    [
        ("Exemplo", new()
        {
            Key = "COZF2TE2BEWGHEB77A5THFYHPBC2KHPM",
            QrCodeBase64 = @"data:image/png;base64,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",
        }),
    ];
}
