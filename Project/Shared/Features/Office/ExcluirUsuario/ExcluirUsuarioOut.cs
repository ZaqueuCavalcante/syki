namespace Exato.Shared.Features.Office.ExcluirUsuario;

public class ExcluirUsuarioOut : IApiDto<ExcluirUsuarioOut>
{
    public static IEnumerable<(string, ExcluirUsuarioOut)> GetExamples() =>
    [
        ("Exemplo", new ExcluirUsuarioOut()),
    ];
}
