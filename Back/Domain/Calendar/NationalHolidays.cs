namespace Estud.Back.Domain.Calendar;

/// <summary>
/// Feriados nacionais brasileiros, fixos e móveis.
/// </summary>
public static class NationalHolidays
{
    public static Dictionary<DateOnly, string> OfYear(int year)
    {
        var easter = Easter(year);

        return new Dictionary<DateOnly, string>
        {
            [new DateOnly(year, 1, 1)] = "Confraternização Universal",
            [easter.AddDays(-48)] = "Carnaval",
            [easter.AddDays(-47)] = "Carnaval",
            [easter.AddDays(-2)] = "Sexta-feira Santa",
            [new DateOnly(year, 4, 21)] = "Tiradentes",
            [new DateOnly(year, 5, 1)] = "Dia do Trabalho",
            [easter.AddDays(60)] = "Corpus Christi",
            [new DateOnly(year, 9, 7)] = "Independência do Brasil",
            [new DateOnly(year, 10, 12)] = "Nossa Senhora Aparecida",
            [new DateOnly(year, 11, 2)] = "Finados",
            [new DateOnly(year, 11, 15)] = "Proclamação da República",
            [new DateOnly(year, 11, 20)] = "Consciência Negra",
            [new DateOnly(year, 12, 25)] = "Natal",
        };
    }

    /// <summary>
    /// Domingo de Páscoa pelo algoritmo de Meeus/Jones/Butcher.
    /// </summary>
    private static DateOnly Easter(int year)
    {
        var a = year % 19;
        var b = year / 100;
        var c = year % 100;
        var d = b / 4;
        var e = b % 4;
        var f = (b + 8) / 25;
        var g = (b - f + 1) / 3;
        var h = ((19 * a) + b - d - g + 15) % 30;
        var i = c / 4;
        var k = c % 4;
        var l = (32 + (2 * e) + (2 * i) - h - k) % 7;
        var m = (a + (11 * h) + (22 * l)) / 451;

        var month = (h + l - (7 * m) + 114) / 31;
        var day = ((h + l - (7 * m) + 114) % 31) + 1;

        return new DateOnly(year, month, day);
    }
}
