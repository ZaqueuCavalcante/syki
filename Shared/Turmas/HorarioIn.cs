namespace Syki.Shared;

public class HorarioIn
{
    public Dia Dia { get; set; }
    public Hora Start { get; set; }
    public Hora End { get; set; }

    public HorarioIn() {}

    public HorarioIn(
        Dia dia,
        Hora start,
        Hora end
    ) {
        Dia = dia;
        Start = start;
        End = end;
    }
}
