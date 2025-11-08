using MudBlazor;

namespace Exato.Front.Components.Usuarios;

public class PotencialEmpresaDoUsuarioSelectData
{
    public int Id { get; set; }
    public bool Ativa { get; set; }
    public string CNPJ { get; set; }
    public string Nome { get; set; }
    public TipoDeEmpresa Tipo { get; set; }

    public int? GetId()
    {
        return Id == 0 ? null : Id;
    }

    public (Color color, string icon) GetIsActive()
    {
        if (Ativa) return (Color.Success, Icons.Material.Filled.CheckCircle);

        return (Color.Default, Icons.Material.Filled.RemoveCircleOutline);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return Id == ((PotencialEmpresaDoUsuarioSelectData)obj).Id;
    }

    public override int GetHashCode()
    {
        return Id;
    }

    public override string ToString()
    {
        return Nome;
    }
}
