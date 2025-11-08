namespace Exato.Shared.Features.Office.BuscarUsuarios;

public class BuscarUsuariosIn : IApiDto<BuscarUsuariosIn>
{
    public int Page { get; set; }
    public bool? IsActive { get; set; }
    public string? SearchTerm { get; set; }

    public static IEnumerable<(string, BuscarUsuariosIn)> GetExamples() =>
    [
        ("Exemplo", new BuscarUsuariosIn() { }),
    ];
}
