namespace Syki.Back.Features.Academic.CreateEnrollmentPeriod;

public class EnrollmentPeriod
{
    public string Id { get; set; }
    public Guid InstitutionId { get; set; }
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }

    public EnrollmentPeriod(
        string id,
        Guid institutionId,
        DateOnly start,
        DateOnly end
    ) {
        Id = id;
        InstitutionId = institutionId;

        if (start >= end)
            Throw.DE023.Now();

        Start = start;
        End = end;
    }

    public EnrollmentPeriodOut ToOut()
    {
        return new EnrollmentPeriodOut
        {
            Id = Id,
            Start = Start,
            End = End,
        };
    }
}
