namespace Syki.Shared;

public class HorarioOut
{
    public Day Day { get; set; }
    public Hora Start { get; set; }
    public Hora End { get; set; }

    public HorarioOut() { }

    public HorarioOut(
        Day dia,
        Hora start,
        Hora end
    ) {
        Day = dia;
        Start = start;
        End = end;
    }
}
