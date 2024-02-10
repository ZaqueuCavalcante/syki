namespace Syki.Shared;

public class HorarioOut
{
    public Dia Dia { get; set; }
    public Hora Start { get; set; }
    public Hora End { get; set; }

    public HorarioOut() { }

    public HorarioOut(
        Dia dia,
        Hora start,
        Hora end
    ) {
        Dia = dia;
        Start = start;
        End = end;
    }
}
