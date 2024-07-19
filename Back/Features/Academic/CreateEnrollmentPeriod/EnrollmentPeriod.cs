namespace Syki.Back.Features.Academic.CreateEnrollmentPeriod;

public class EnrollmentPeriod
{
    public string Id { get; set; }
    public Guid InstitutionId { get; set; }
    public DateOnly StartAt { get; set; }
    public DateOnly EndAt { get; set; }

    public EnrollmentPeriod(
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

    public static OneOf<EnrollmentPeriod, SykiError> New(
        string id,
        Guid institutionId,
        DateOnly startAt,
        DateOnly endAt
    ) {
        if (startAt >= endAt)
            return new EnrollmentPeriodStartDateShouldBeLessThanEndDate();

        return new EnrollmentPeriod(id, institutionId, startAt, endAt);
    }

    public EnrollmentPeriodOut ToOut()
    {
        return new EnrollmentPeriodOut
        {
            Id = Id,
            StartAt = StartAt,
            EndAt = EndAt,
        };
    }
}
