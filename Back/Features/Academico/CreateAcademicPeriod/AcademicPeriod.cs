namespace Syki.Back.Features.Academic.CreateAcademicPeriod;

public class AcademicPeriod
{
    public string Id { get; set; }
    public Guid InstitutionId { get; set; }
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }

    public AcademicPeriod(
        string id,
        Guid institutionId,
        DateOnly start,
        DateOnly end
    ) {
        Id = Validate(id, start, end);
        InstitutionId = institutionId;
        Start = start;
        End = end;
    }

    private static string Validate(
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

    public AcademicPeriodOut ToOut()
    {
        return new AcademicPeriodOut
        {
            Id = Id,
            Start = Start,
            End = End,
        };
    }
}
