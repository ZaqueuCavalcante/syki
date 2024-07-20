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
        Id = id;
        InstitutionId = institutionId;
        StartAt = startAt;
        EndAt = endAt;
    }

    public static OneOf<AcademicPeriod, SykiError> New(
        string id,
        Guid institutionId,
        DateOnly startAt,
        DateOnly endAt
    ) {
        var result = Validate(id, startAt, endAt);

        return result.Match<OneOf<AcademicPeriod, SykiError>>(
            validId => new AcademicPeriod(validId, institutionId, startAt, endAt),
            error => error
        );
    }

    private static OneOf<string, SykiError> Validate(
        string id,
        DateOnly startAt,
        DateOnly endAt
    ) {
        var numbers = id.OnlyNumbers();

        if (numbers.Length != 5)
            return new InvalidAcademicPeriod();

        var year = int.Parse(numbers.Substring(0, 4));
        var digit = int.Parse(numbers.Substring(4, 1));

        if (year < 1970 || year > 2070)
            return new InvalidAcademicPeriod();

        if (digit < 1 || digit > 2)
            return new InvalidAcademicPeriod();

        if (startAt.Year != year)
            return new InvalidAcademicPeriodStartDate();

        if (endAt.Year != year)
            return new InvalidAcademicPeriodEndDate();

        if (startAt >= endAt)
            return new AcademicPeriodStartDateShouldBeLessThanEndDate();

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
