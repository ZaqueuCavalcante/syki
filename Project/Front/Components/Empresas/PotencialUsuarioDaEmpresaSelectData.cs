using MudBlazor;

namespace Exato.Front.Components.Empresas;

public class PotencialUsuarioDaEmpresaSelectData
{
    public int Id { get; set; }
    public bool Active { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public int? GetId()
    {
        return Id == 0 ? null : Id;
    }

    public (Color color, string icon) GetIsActive()
    {
        if (Active) return (Color.Success, Icons.Material.Filled.CheckCircle);

        return (Color.Default, Icons.Material.Filled.RemoveCircleOutline);
    }

    public override string ToString()
    {
        return Name;
    }
}
