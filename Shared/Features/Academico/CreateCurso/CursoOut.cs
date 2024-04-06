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
        return Id.ToHashCode();
    }

    public override string ToString()
    {
        return Nome;
    }
}
