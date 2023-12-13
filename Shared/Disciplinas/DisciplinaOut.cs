namespace Syki.Shared;

public class DisciplinaOut
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public byte Periodo { get; set; }
    public byte Creditos { get; set; }
    public ushort CargaHoraria { get; set; }
    public List<Guid> Cursos { get; set; } = [];

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((DisciplinaOut)obj).Id;
    }

    public override int GetHashCode()
    {
        string justNumbers = Id.ToString().OnlyNumbers();
        return int.Parse(justNumbers[..8]);
    }

    public override string ToString()
    {
        return Nome;
    }
}
