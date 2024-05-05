namespace Front.Features.Academico.CreateTurma;

public class HorarioInFillable
{
    public Guid Id { get; set; }
    public Day? Day { get; set; }
    public Hora? Start { get; set; }
    public Hora? End { get; set; }

    public HorarioInFillable()
    {
        Id = Guid.NewGuid();
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((HorarioInFillable)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.ToHashCode();
    }
}
