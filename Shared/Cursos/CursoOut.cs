namespace Syki.Shared;

public class CursoOut
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public TipoDeCurso Tipo { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return this.Id == ((CursoOut)obj).Id;
    }

    public override int GetHashCode()
    {
        string justNumbers = new String(Id.ToString().Where(Char.IsDigit).ToArray());
        return int.Parse(justNumbers.Substring(0, 8));
    }

    public override string ToString()
    {
        return Nome;
    }
}
