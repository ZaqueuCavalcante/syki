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
            throw new DomainException(ExceptionMessages.DE0003);

        var year = int.Parse(numbers.Substring(0, 4));
        var digit = int.Parse(numbers.Substring(4, 1));

        if (year < 1970 || year > 2070)
            throw new DomainException(ExceptionMessages.DE0003);

        if (digit < 1 || digit > 2)
            throw new DomainException(ExceptionMessages.DE0003);

        if (start.Year != year)
            throw new DomainException(ExceptionMessages.DE0004);

        if (end.Year != year)
            throw new DomainException(ExceptionMessages.DE0005);

        if (!(start < end))
            throw new DomainException(ExceptionMessages.DE0006);

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
