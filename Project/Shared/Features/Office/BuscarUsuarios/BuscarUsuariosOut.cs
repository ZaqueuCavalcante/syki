namespace Exato.Shared.Features.Office.BuscarUsuarios;

public class BuscarUsuariosOut : IApiDto<BuscarUsuariosOut>
{
    public int Total { get; set; }
    public List<BuscarUsuariosItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, BuscarUsuariosOut)> GetExamples() =>
    [
        ("Exemplo", new BuscarUsuariosOut() { }),
    ];
}

public class BuscarUsuariosItemOut
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Documento { get; set; }
    public bool Ativo { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? LastAccessAt { get; set; }
}
