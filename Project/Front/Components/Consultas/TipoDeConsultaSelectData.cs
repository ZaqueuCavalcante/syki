using MudBlazor;

namespace Exato.Front.Components.Consultas;

public class TipoDeConsultaSelectData
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public bool Visivel { get; set; }
    public bool Disponivel { get; set; }

    public int? GetId()
    {
        return Id == 0 ? null : Id;
    }

    public (Color color, string visivel) GetVisivel()
    {
        if (Visivel) return (Color.Success, "Visível");

        return (Color.Default, "Visível");
    }

    public (Color color, string disponivel) GetDisponivel()
    {
        if (Disponivel) return (Color.Success, "Disponível");

        return (Color.Default, "Disponível");
    }

    public override string ToString()
    {
        if (Id == 0) return null;

        return $"{Nome} - [{Id}]";
    }
}
