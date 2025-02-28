namespace Syki.Shared;

public class CreateAcademicPeriodIn
{
    public string? Id { get; set; }

    /// <summary>
    /// Data de início
    /// </summary>
    public DateOnly StartAt { get; set; }

    /// <summary>
    /// Data de término
    /// </summary>
    public DateOnly EndAt { get; set; }

    public CreateAcademicPeriodIn() {}

    public CreateAcademicPeriodIn(string id)
    {
        Id = id;
        var numbers = id.OnlyNumbers();
        if (numbers.Length != 5) return;
        var year = int.Parse(numbers.Substring(0, 4));
        var digit = int.Parse(numbers.Substring(4, 1));
        StartAt = digit == 1 ? new DateOnly(year, 02, 01) : new DateOnly(year, 06, 01);
        EndAt = digit == 1 ? new DateOnly(year, 07, 01) : new DateOnly(year, 12, 01);
    }
}
