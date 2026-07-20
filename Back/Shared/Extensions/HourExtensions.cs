namespace Estud.Back.Shared;

public static class HourExtensions
{
    // O valor do enum Hour é HHMM (ex: H07_30 = 730), então hora e minuto saem
    // da divisão e do resto por 100.
    public static int ToMinutes(this Hour hour)
    {
        var value = (int)hour;
        return value / 100 * 60 + value % 100;
    }
}
