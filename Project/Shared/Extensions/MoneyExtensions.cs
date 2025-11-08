using System.Globalization;

namespace Exato.Shared.Extensions;

public static class MoneyExtensions
{
    public static string ToBrl(this int cents, bool withSymbol = true)
    {
        decimal amount = cents / 100m;

        return amount.ToBrl(withSymbol);
    }

    public static string ToBrl(this decimal amount, bool withSymbol = true)
    {
        var br = (CultureInfo)CultureInfo.GetCultureInfo("pt-BR").Clone();
        br.NumberFormat.CurrencySymbol = "R$ ";

        if (withSymbol)
            return amount.ToString("C", br);

        return amount.ToString("N2", br);
    }
}
