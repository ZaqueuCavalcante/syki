namespace Syki.Shared;

public class TurmaOut
{
    public Guid Id { get; set; }
    public string Disciplina { get; set; }
    public string Professor { get; set; }
    public string Periodo { get; set; }
    public string Horario { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((TurmaOut)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.ToHashCode();
    }
}
