using QRCoder;
using Syki.Back.Domain.Identity;

namespace Syki.Back.Features.Identity.GetTwoFactorKey;

public class GetTwoFactorKeyService(SykiDbContext ctx, UserManager<SykiUser> userManager) : ISykiService
{
    public async Task<GetTwoFactorKeyOut> Get()
    {
        var webUser = await userManager.Users.FirstAsync(u => u.Id == ctx.RequestUser.Id);

        var key = await userManager.GetAuthenticatorKeyAsync(webUser);

        if (key == null)
        {
            await userManager.ResetAuthenticatorKeyAsync(webUser);
            key = await userManager.GetAuthenticatorKeyAsync(webUser);
        }

        return new()
        {
            Key = key!,
            TwoFactorEnabled = webUser.TwoFactorEnabled,
            QrCodeBase64 = GenerateQrCodeBase64(key, webUser.Email)
        };
    }

    private static string GenerateQrCodeBase64(string key, string email)
    {
        const string provider = "Exato";

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
