using Microsoft.AspNetCore.DataProtection;

namespace Syki.Back.Auth.Managers;

public class SsoEncryptionManager(IDataProtectionProvider provider)
{
    private readonly IDataProtector _protector = provider.CreateProtector("SsoClientSecrets");

    public string Encrypt(string plainText) => _protector.Protect(plainText);

    public string Decrypt(string cipherText) => _protector.Unprotect(cipherText);
}
