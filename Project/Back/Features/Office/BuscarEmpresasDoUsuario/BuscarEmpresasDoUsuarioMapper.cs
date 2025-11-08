using Exato.Back.Intelligence.Domain.Public;
using Exato.Shared.Features.Office.BuscarEmpresasDoUsuario;

namespace Exato.Back.Features.Office.BuscarEmpresasDoUsuario;

public static class BuscarEmpresasDoUsuarioMapper
{
    extension(Cliente cliente)
    {
        public BuscarEmpresasDoUsuarioItemOut ToBuscarEmpresasDoUsuarioItemOut()
        {
            return new()
            {
                Id = cliente.ClienteId,
                Nome = cliente.Nome,
                Tipo = cliente.GetTipo(),
                CNPJ = cliente.GetDocument() ?? "-",
                Ativa = cliente.Ativo,
            };
        }
    }
}
