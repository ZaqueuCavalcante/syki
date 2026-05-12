using Bogus;
using OtpNet;

namespace Syki.Tests.Integration.Base;

public static class DataGen
{
    public static string Numbers => $"{Guid.NewGuid().ToString().OnlyNumbers()}";

    public static string Email => $"{Numbers}.test@syki.com.br";

    public static string UserName => new Faker("pt_BR").Person.FullName;

    public static string GenerateTOTP(this string key)
    {
        var totp = new Totp(Base32Encoding.ToBytes(key));
        return totp.ComputeTotp();
    }
}
