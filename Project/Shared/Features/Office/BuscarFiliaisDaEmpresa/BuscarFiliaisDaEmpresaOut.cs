namespace Exato.Shared.Features.Office.BuscarFiliaisDaEmpresa;

public class BuscarFiliaisDaEmpresaOut : IApiDto<BuscarFiliaisDaEmpresaOut>
{
    public List<BuscarFiliaisDaEmpresaItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, BuscarFiliaisDaEmpresaOut)> GetExamples() =>
    [
        ("Exemplo", new BuscarFiliaisDaEmpresaOut() { }),
    ];
}

public class BuscarFiliaisDaEmpresaItemOut : IEquatable<BuscarFiliaisDaEmpresaItemOut>
{
    public int Id { get; set; }
    public string Nome { get; set; }

    public List<BuscarFiliaisDaEmpresaItemOut> Filiais { get; set; } = [];

    public bool Equals(BuscarFiliaisDaEmpresaItemOut? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is BuscarFiliaisDaEmpresaItemOut other && Equals(other);

    public override int GetHashCode() => Id;
}
