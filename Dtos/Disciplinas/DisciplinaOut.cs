namespace Syki.Dtos;

public class DisciplinaOut
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public byte Periodo { get; set; }
    public byte Creditos { get; set; }
    public ushort CargaHoraria { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((DisciplinaOut)obj).Id;
    }

    public override int GetHashCode()
    {
        return (int)Id;
    }
}
