namespace Syki.Shared;

public class PeriodoDeMatriculaIn
{
    public string Id { get; set; }
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }

    public PeriodoDeMatriculaIn() {}

    public PeriodoDeMatriculaIn(string id, string start, string end)
    {
        Id = id;
        var year = int.Parse(id.OnlyNumbers().Substring(0, 4));

        start = start.OnlyNumbers();
        var startDay = int.Parse(start.Substring(0, 2));
        var startMonth = int.Parse(start.Substring(2, 2));
        Start = new DateOnly(year, startMonth, startDay);

        end = end.OnlyNumbers();
        var endDay = int.Parse(end.Substring(0, 2));
        var endMonth = int.Parse(end.Substring(2, 2));
        End = new DateOnly(year, endMonth, endDay);
    }
}
