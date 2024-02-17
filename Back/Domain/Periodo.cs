using Syki.Shared;
using Syki.Back.Exceptions;

namespace Syki.Back.Domain;

public class Periodo
{
    public string Id { get; set; }
    public Guid FaculdadeId { get; set; }
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }

    public Periodo(
        string id,
        Guid faculdadeId,
        DateOnly start,
        DateOnly end
    ) {
        Id = Validate(id, start, end);
        FaculdadeId = faculdadeId;
        Start = start;
        End = end;
    }

    private string Validate(
        string id,
        DateOnly start,
        DateOnly end
    ) {
        var numbers = id.OnlyNumbers();

        if (numbers.Length != 5)
            Throw.DE006.Now();

        var year = int.Parse(numbers.Substring(0, 4));
        var digit = int.Parse(numbers.Substring(4, 1));

        if (year < 1970 || year > 2070)
            Throw.DE006.Now();

        if (digit < 1 || digit > 2)
            Throw.DE006.Now();

        if (start.Year != year)
            Throw.DE007.Now();

        if (end.Year != year)
            Throw.DE008.Now();

        if (start >= end)
            Throw.DE009.Now();

        return $"{year}.{digit}";
    }

    public PeriodoOut ToOut()
    {
        return new PeriodoOut
        {
            Id = Id,
            Start = Start,
            End = End,
        };
    }
}
