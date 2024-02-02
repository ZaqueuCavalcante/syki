namespace Syki.Shared;

public class AgendaDiaOut
{
    public Dia Dia { get; set; }
    public List<AgendaDisciplinaOut> Disciplinas { get; set; }
}

public class AgendaDisciplinaOut
{
    public string Nome { get; set; }
    public Hora Start { get; set; }
    public Hora End { get; set; }
}
