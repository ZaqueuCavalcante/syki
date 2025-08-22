namespace Syki.Shared;

public class AgendaDayOut
{
    public Day Day { get; set; }
    public List<AgendaDisciplineOut> Disciplines { get; set; } = [];
}

public class AgendaDisciplineOut
{
    public Guid? ClassId { get; set; }
    public string Name { get; set; }
    public Hour Start { get; set; }
    public Hour End { get; set; }
}
