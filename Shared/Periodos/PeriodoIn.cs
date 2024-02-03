namespace Syki.Shared;

public class PeriodoIn
{
    public string Id { get; set; }
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }

    public PeriodoIn() {}

    public PeriodoIn(string id)
    {
        Id = id;
        var numbers = id.OnlyNumbers();
        var year = int.Parse(numbers.Substring(0, 4));
        var digit = int.Parse(numbers.Substring(4, 1));
        Start = digit == 1 ? new DateOnly(year, 02, 01) : new DateOnly(year, 06, 01);
        End = digit == 1 ? new DateOnly(year, 07, 01) : new DateOnly(year, 12, 01);
    }
}
