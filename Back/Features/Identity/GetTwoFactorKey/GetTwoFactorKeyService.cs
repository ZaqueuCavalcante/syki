using QRCoder;
using Estud.Back.Domain.Identity;

namespace Estud.Back.Features.Identity.GetTwoFactorKey;

public class GetTwoFactorKeyService(EstudDbContext ctx, UserManager<EstudUser> userManager) : IEstudService
{
    public async Task<GetTwoFactorKeyOut> Get()
    {
        var userId = ctx.RequestUser.Id;
        var key = await ctx.GetUserTwoFactorKeyAsync(userId);
        var user = await ctx.Users.Where(u => u.Id == userId).Select(x => new { x.Email, x.TwoFactorEnabled }).FirstOrDefaultAsync();

        if (key == null)
        {
            var estudUser = await userManager.Users.FirstAsync(u => u.Id == ctx.RequestUser.Id);
            await userManager.ResetAuthenticatorKeyAsync(estudUser);
            key = await userManager.GetAuthenticatorKeyAsync(estudUser);
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
        const string provider = "Estud";

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
