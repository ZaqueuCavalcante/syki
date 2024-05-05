namespace Syki.Shared;

public class AgendaDayOut
{
    public Day Day { get; set; }
    public List<AgendaDisciplineOut> Disciplines { get; set; } = [];
}

public class AgendaDisciplineOut
{
    public string Name { get; set; }
    public Hora Start { get; set; }
    public Hora End { get; set; }
}
