namespace Exato.Shared.Features.Office.CriarUsuario;

public class CriarUsuarioOut : IApiDto<CriarUsuarioOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, CriarUsuarioOut)> GetExamples() =>
    [
        ("Exemplo", new CriarUsuarioOut() { }),
    ];
}
