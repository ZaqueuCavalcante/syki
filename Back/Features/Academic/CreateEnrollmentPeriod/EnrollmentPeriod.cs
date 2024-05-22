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

        if (startAt >= endAt)
            Throw.DE023.Now();

        StartAt = startAt;
        EndAt = endAt;
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
