namespace Exato.Shared.Features.Office.EditarClaimsDoUsuario;

public class EditarClaimsDoUsuarioIn : IApiDto<EditarClaimsDoUsuarioIn>
{
    public List<ExatoWebClaims> Claims { get; set; }

    public static IEnumerable<(string, EditarClaimsDoUsuarioIn)> GetExamples() =>
    [
        ("Exemplo", new EditarClaimsDoUsuarioIn() { }),
    ];
}
