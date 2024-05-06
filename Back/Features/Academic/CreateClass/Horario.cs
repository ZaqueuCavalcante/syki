namespace Syki.Back.CreateTurma;

public class Horario
{
    public Guid Id { get; set; }
    public Guid TurmaId { get; set; }
    public Day Day { get; set; }
    public Hora Start { get; set; }
    public Hora End { get; set; }

    private Horario() {}

    public Horario(
        Day dia,
        Hora start,
        Hora end
    ) {
        Id = Guid.NewGuid();
        Day = dia;

        if (start == end || end < start)
            Throw.DE021.Now();

        Start = start;
        End = end;
    }

    public bool Conflict(Horario other)
    {
        if (Day != other.Day)
            return false;

        if (Start == other.Start || End == other.End)
            return true;

        if (Start < other.Start && other.Start < End)
            return true;

        if (Start < other.End && other.End < End)
            return true;

        if (other.Start < Start && Start < other.End)
            return true;

        if (other.Start < End && End < other.End)
            return true;

        return false;
    }

    public override string ToString()
    {
        return $"{Day.GetDescription()} {Start.GetDescription()}-{End.GetDescription()}";
    }

    public HorarioOut ToOut()
    {
        return new HorarioOut
        {
            Day = Day,
            Start = Start,
            End = End,
        };
    }
}
