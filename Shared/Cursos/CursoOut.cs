namespace Syki.Shared;

public class CursoOut
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public TipoDeCurso Tipo { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((CursoOut)obj).Id;
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
