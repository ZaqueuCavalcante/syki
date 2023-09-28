namespace Syki.Dtos;

public class DisciplinaOut
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public ushort CargaHoraria { get; set; }

    public bool IsSelected { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return this.Id == ((DisciplinaOut)obj).Id;
    }

    public override int GetHashCode()
    {
        return (int)Id;
    }
}
