namespace Estud.Back.Domain.Periods;

public class AcademicPeriod
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public string Name { get; set; }
    public DateOnly StartAt { get; set; }
    public DateOnly EndAt { get; set; }

    private AcademicPeriod() {}

    public AcademicPeriod(
        int institutionId,
        string name,
        DateOnly startAt,
        DateOnly endAt
    ) {
        InstitutionId = institutionId;
        Name = name;
        StartAt = startAt;
        EndAt = endAt;
    }

    public static OneOf<AcademicPeriod, EstudError> New(
        int institutionId,
        string name,
        DateOnly startAt,
        DateOnly endAt
    ) {
        var result = Validate(name, startAt, endAt);

        if (result.IsError) return result.Error;

        return new AcademicPeriod(institutionId, result.Success, startAt, endAt);
    }

    private static OneOf<string, EstudError> Validate(
        string name,
        DateOnly startAt,
        DateOnly endAt
    ) {
        var numbers = name.OnlyNumbers();

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
            return new InvalidAcademicPeriodDates();

        return $"{year}.{digit}";
    }
}
