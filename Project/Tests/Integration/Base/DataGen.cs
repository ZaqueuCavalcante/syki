using Bogus;
using OtpNet;
using Bogus.Extensions.Brazil;

namespace Exato.Tests.Integration.Base;

public static class DataGen
{
    public static string Email => $"{Guid.NewGuid().ToString().OnlyNumbers()}.test@exato.com.br";
    public static string Numbers => $"{Guid.NewGuid().ToString().OnlyNumbers()}";

    public static string Cpf => new Faker("pt_BR").Person.Cpf();
    public static string UserName => new Faker("pt_BR").Person.FullName;

    public static string Cnpj => new Faker("pt_BR").Company.Cnpj();
    public static string OrgName => new Faker("pt_BR").Company.CompanyName();

    public static string GenerateTOTP(this string key)
    {
        var totp = new Totp(Base32Encoding.ToBytes(key));
        return totp.ComputeTotp();
    }
}
