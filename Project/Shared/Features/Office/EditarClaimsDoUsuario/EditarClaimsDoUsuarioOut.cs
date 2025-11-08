namespace Exato.Shared.Features.Office.EditarClaimsDoUsuario;

public class EditarClaimsDoUsuarioOut : IApiDto<EditarClaimsDoUsuarioOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, EditarClaimsDoUsuarioOut)> GetExamples() =>
    [
        ("Exemplo", new EditarClaimsDoUsuarioOut() { }),
    ];
}
