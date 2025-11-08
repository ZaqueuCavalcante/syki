using MudBlazor;

namespace Exato.Front.Components.Empresas;

public class EmpresaSelectData
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

    public override string ToString()
    {
        return Nome;
    }
}
