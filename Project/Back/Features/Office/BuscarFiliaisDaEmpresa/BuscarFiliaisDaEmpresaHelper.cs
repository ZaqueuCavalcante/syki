using Exato.Shared.Features.Office.BuscarFiliaisDaEmpresa;

namespace Exato.Back.Features.Office.BuscarFiliaisDaEmpresa;

public static class BuscarFiliaisDaEmpresaHelper
{
    public static List<BuscarFiliaisDaEmpresaItemOut> OrdenarHierarquicamente(this IEnumerable<EmpresaFilialDto> filiais, int id)
    {
        var filiaisDiretas = filiais
            .GroupBy(f => f.ParentId)
            .ToDictionary(g => g.Key, g => g.ToList());

        // Proteção contra ciclos (dados ruins)
        var visitados = new HashSet<int>();

        List<BuscarFiliaisDaEmpresaItemOut> MontarFiliais(int parentId)
        {
            if (!filiaisDiretas.TryGetValue(parentId, out var filhosDto) || filhosDto.Count == 0)
                return [];

            var lista = new List<BuscarFiliaisDaEmpresaItemOut>(filhosDto.Count);
            foreach (var dto in filhosDto)
            {
                if (!visitados.Add(dto.Id))
                    throw new InvalidOperationException($"Ciclo detectado na hierarquia (Id {dto.Id}).");

                var no = new BuscarFiliaisDaEmpresaItemOut
                {
                    Id = dto.Id,
                    Nome = dto.Nome,
                    Filiais = MontarFiliais(dto.Id)
                };

                visitados.Remove(dto.Id);
                lista.Add(no);
            }

            return lista.OrderByDescending(x => x.Filiais?.Count ?? 0).ToList();
        }

        return MontarFiliais(id);
    }
}
