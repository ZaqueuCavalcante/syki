namespace Syki.Shared;

public class CreateEnrollmentPeriodIn
{
    public string Id { get; set; }
    public DateOnly StartAt { get; set; }
    public DateOnly EndAt { get; set; }

    public CreateEnrollmentPeriodIn() {}

    public CreateEnrollmentPeriodIn(string id, string start, string end)
    {
        Id = id;
        var year = int.Parse(id.OnlyNumbers().Substring(0, 4));

        start = start.OnlyNumbers();
        var startDay = int.Parse(start.Substring(0, 2));
        var startMonth = int.Parse(start.Substring(2, 2));
        StartAt = new DateOnly(year, startMonth, startDay);

        end = end.OnlyNumbers();
        var endDay = int.Parse(end.Substring(0, 2));
        var endMonth = int.Parse(end.Substring(2, 2));
        EndAt = new DateOnly(year, endMonth, endDay);
    }
}
