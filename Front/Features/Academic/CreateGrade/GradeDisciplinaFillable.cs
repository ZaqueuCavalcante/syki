namespace Syki.Front.Domain;

public class GradeDisciplinaFillable
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public byte? Periodo { get; set; }
    public byte? Creditos { get; set; }
    public ushort? CargaHoraria { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((GradeDisciplinaFillable)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id.ToHashCode();
    }

    public override string ToString()
    {
        return Name;
    }
}
