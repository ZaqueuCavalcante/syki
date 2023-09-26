namespace Syki.Dtos;

public class CursoOut
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public TipoDeCurso Tipo { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return this.Id == ((CursoOut)obj).Id;
    }

    public override int GetHashCode()
    {
        return (int)Id;
    }

    public override string ToString()
    {
        return Nome;
    }
}
