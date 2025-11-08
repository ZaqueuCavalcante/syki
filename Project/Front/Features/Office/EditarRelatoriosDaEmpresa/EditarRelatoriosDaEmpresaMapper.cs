using Exato.Shared.Features.Office.BuscarEmpresa;

namespace Exato.Front.Features.Office.EditarRelatoriosDaEmpresa;

public static class EditarRelatoriosDaEmpresaMapper
{
    extension(BuscarEmpresaRelatorioOut relatorio)
    {
        public EditarRelatoriosDaEmpresaItemFormData ToItemFormData()
        {
            return new()
            {
                Id = relatorio.Id,
                Nome = relatorio.Nome,
            };
        }
    }
}
