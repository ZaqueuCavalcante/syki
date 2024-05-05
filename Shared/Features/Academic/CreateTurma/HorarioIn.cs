namespace Syki.Shared;

public class HorarioIn
{
    public Guid Id { get; set; }
    public Day Day { get; set; }
    public Hora Start { get; set; }
    public Hora End { get; set; }

    public HorarioIn()
    {
        Id = Guid.NewGuid();
    }

    public HorarioIn(
        Day dia,
        Hora start,
        Hora end
    ) {
        Id = Guid.NewGuid();
        Day = dia;
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

        return other.Day == Day && other.Start == Start && other.End == End;
    }

    public override int GetHashCode()
    {
        return Id.ToHashCode();
    }
}
