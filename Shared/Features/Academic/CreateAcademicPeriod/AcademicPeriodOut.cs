namespace Syki.Shared;

public class AcademicPeriodOut
{
    public string Id { get; set; }

    /// <summary>
    /// Data de início
    /// </summary>
    public DateOnly StartAt { get; set; }

    /// <summary>
    /// Data de término
    /// </summary>
    public DateOnly EndAt { get; set; }

    public static implicit operator AcademicPeriodOut(OneOf<AcademicPeriodOut, ErrorOut> value)
    {
        return value.GetSuccess();
    }
}
