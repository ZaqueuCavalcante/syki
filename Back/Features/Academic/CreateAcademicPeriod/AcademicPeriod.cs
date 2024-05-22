namespace Syki.Back.Features.Academic.CreateAcademicPeriod;

public class AcademicPeriod
{
    public string Id { get; set; }
    public Guid InstitutionId { get; set; }
    public DateOnly StartAt { get; set; }
    public DateOnly EndAt { get; set; }

    public AcademicPeriod(
        string id,
        Guid institutionId,
        DateOnly startAt,
        DateOnly endAt
    ) {
        Id = Validate(id, startAt, endAt);
        InstitutionId = institutionId;
        StartAt = startAt;
        EndAt = endAt;
    }

    private static string Validate(
        string id,
        DateOnly startAt,
        DateOnly endAt
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

        if (startAt.Year != year)
            Throw.DE007.Now();

        if (endAt.Year != year)
            Throw.DE008.Now();

        if (startAt >= endAt)
            Throw.DE009.Now();

        return $"{year}.{digit}";
    }

    public AcademicPeriodOut ToOut()
    {
        return new AcademicPeriodOut
        {
            Id = Id,
            StartAt = StartAt,
            EndAt = EndAt,
        };
    }
}
