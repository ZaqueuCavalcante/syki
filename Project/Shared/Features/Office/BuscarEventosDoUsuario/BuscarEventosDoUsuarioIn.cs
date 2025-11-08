namespace Exato.Shared.Features.Office.BuscarEventosDoUsuario;

public class BuscarEventosDoUsuarioIn : IApiDto<BuscarEventosDoUsuarioIn>
{
    public int Page { get; set; }

    public static IEnumerable<(string, BuscarEventosDoUsuarioIn)> GetExamples() =>
    [
        ("Exemplo", new BuscarEventosDoUsuarioIn() { }),
    ];
}
