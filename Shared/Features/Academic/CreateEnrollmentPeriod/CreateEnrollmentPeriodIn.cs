namespace Syki.Shared;

public class CreateEnrollmentPeriodIn
{
    public string Id { get; set; }
    public DateOnly StartAt { get; set; }
    public DateOnly EndAt { get; set; }
}
