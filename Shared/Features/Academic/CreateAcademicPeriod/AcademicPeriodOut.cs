namespace Syki.Shared;

public class AcademicPeriodOut
{
    public string Id { get; set; }
    public DateOnly StartAt { get; set; }
    public DateOnly EndAt { get; set; }

    public static implicit operator AcademicPeriodOut(OneOf<AcademicPeriodOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
