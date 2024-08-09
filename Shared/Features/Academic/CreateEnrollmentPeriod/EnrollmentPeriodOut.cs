namespace Syki.Shared;

public class EnrollmentPeriodOut
{
    public string Id { get; set; }
    public DateOnly StartAt { get; set; }
    public DateOnly EndAt { get; set; }

    public static implicit operator EnrollmentPeriodOut(OneOf<EnrollmentPeriodOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
