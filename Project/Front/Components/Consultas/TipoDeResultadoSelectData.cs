namespace Exato.Front.Components.Consultas;

public class TipoDeResultadoSelectData
{
    public int Id { get; set; }
    public string Nome { get; set; }

    public int? GetId()
    {
        return Id == 0 ? null : Id;
    }

    public override string ToString()
    {
        if (Id == 0) return null;

        return $"{Nome} - [{Id}]";
    }
}
