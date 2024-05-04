using QRCoder;

namespace Syki.Shared;

public static class GuidExtensions
{
    public static int ToHashCode(this Guid guid)
    {
        var justNumbers = guid.ToString().OnlyNumbers();
        return int.Parse(justNumbers[..8]);
    }

    public static Byte[] GenerateQrCodeBytes(string key, string email)
    {
        const string provider = "Syki";

        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(
            $"otpauth://totp/{provider}:{email}?secret={key}&issuer={provider}",
            QRCodeGenerator.ECCLevel.Q
        );
        
        var qrCode = new PngByteQRCode(qrCodeData);

        return qrCode.GetGraphic(20);
    }
}
