using Syki.Shared;

namespace Syki.Front.Domain;

public class GradeDisciplinaFull
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public ushort? CargaHoraria { get; set; }
    public byte? Periodo { get; set; }
    public byte? Creditos { get; set; }
    public bool IsSelected { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((GradeDisciplinaFull)obj).Id;
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
