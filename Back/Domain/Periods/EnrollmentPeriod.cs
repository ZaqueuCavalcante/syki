namespace Estud.Back.Domain.Periods;

public class EnrollmentPeriod
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public string Name { get; set; }
    public DateOnly StartAt { get; set; }
    public DateOnly EndAt { get; set; }

    public EnrollmentPeriod(
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

    public static OneOf<EnrollmentPeriod, EstudError> New(
        int institutionId,
        string name,
        DateOnly startAt,
        DateOnly endAt
    ) {
        if (startAt >= endAt) return new InvalidEnrollmentPeriodDates();

        return new EnrollmentPeriod(institutionId, name, startAt, endAt);
    }

    public OneOf<EstudSuccess, EstudError> Update(
        string name,
        DateOnly startAt,
        DateOnly endAt
    ) {
        if (startAt >= endAt) return InvalidEnrollmentPeriodDates.I;

        Name = name;
        StartAt = startAt;
        EndAt = endAt;

        return EstudSuccess.I;
    }
}
