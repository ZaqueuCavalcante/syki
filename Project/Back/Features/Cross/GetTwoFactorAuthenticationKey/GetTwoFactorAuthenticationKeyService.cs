using QRCoder;
using Exato.Back.Features.Cross.CreateExatoUser;
using Exato.Shared.Features.Cross.GetTwoFactorAuthenticationKey;

namespace Exato.Back.Features.Cross.GetTwoFactorAuthenticationKey;

public class GetTwoFactorAuthenticationKeyService(UserManager<ExatoUser> userManager) : ICrossService
{
    public async Task<GetTwoFactorAuthenticationKeyOut> Get(Guid userId)
    {
        var user = await userManager.Users.FirstAsync(u => u.Id == userId);

        var key = await userManager.GetAuthenticatorKeyAsync(user);

        if (key == null)
        {
            await userManager.ResetAuthenticatorKeyAsync(user);
            key = await userManager.GetAuthenticatorKeyAsync(user);
        }

        return new()
        {
            Key = key!,
            TwoFactorEnabled = user.TwoFactorEnabled,
            QrCodeBase64 = GenerateQrCodeBase64(key, user.Email)
        };
    }

    private static string GenerateQrCodeBase64(string key, string email)
    {
        const string provider = "Exato Admin";

        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(
            $"otpauth://totp/{provider}:{email}?secret={key}&issuer={provider}",
            QRCodeGenerator.ECCLevel.Q
        );

        var qrCode = new PngByteQRCode(qrCodeData);

        var bytes = qrCode.GetGraphic(20);

        return string.Format("data:image/png;base64,{0}", Convert.ToBase64String(bytes));
    }
}
