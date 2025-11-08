namespace Exato.Shared.Extensions;

public static class PhoneNumberExtensions
{
    public static bool IsValidPhoneNumber(this string phone)
    {
        phone = phone.OnlyNumbers();

        if (phone.Length < 10 || phone.Length > 11) return false;

        return true;
    }

    public static string AsFormatedPhoneNumber(this string phone)
    {
        phone = phone.OnlyNumbers();

        if (phone.IsValidPhoneNumber())
        {
            if (phone.Length == 10) phone = phone.Insert(2, "9");
            return phone.MaskNumber("(##) #####-####");
        }

        return phone;
    }
}
