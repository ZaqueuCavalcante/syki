namespace Syki.Shared;

public class HorarioIn
{
    public Guid Id { get; set; }
    public Dia Dia { get; set; }
    public Hora Start { get; set; }
    public Hora End { get; set; }

    public HorarioIn()
    {
        Id = Guid.NewGuid();
    }

    public HorarioIn(
        Dia dia,
        Hora start,
        Hora end
    ) {
        Id = Guid.NewGuid();
        Dia = dia;
        Start = start;
        End = end;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        var other = (HorarioIn) obj;

        if (other.Id == Id)
            return true;

        return other.Dia == Dia && other.Start == Start && other.End == End;
    }

    public override int GetHashCode()
    {
        return Id.ToHashCode();
    }

    // TODO: refactor this to use same Horario method
    public bool Conflict(HorarioIn other)
    {
        if (Dia != other.Dia)
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
}
