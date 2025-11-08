using MudBlazor;

namespace Exato.Front.Components.Empresas;

public static class EmpresasMapper
{
    extension(TipoDeEmpresa tipo)
    {
        public (Color color, string icon, string tipo) GetProps()
        {
            if (tipo == TipoDeEmpresa.Matriz) return (Color.Primary, Icons.Material.Filled.Museum, "Matrizes");
            if (tipo == TipoDeEmpresa.Filial) return (Color.Secondary, Icons.Material.Filled.OtherHouses, "Filiais");

            return (Color.Default, Icons.Material.Filled.Adjust, "Avulsas");
        }
    }
}
