namespace Exato.Shared.Features.Office.EditarCadastroDoUsuario;

public class EditarCadastroDoUsuarioOut : IApiDto<EditarCadastroDoUsuarioOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, EditarCadastroDoUsuarioOut)> GetExamples() =>
    [
        ("Exemplo", new EditarCadastroDoUsuarioOut() { }),
    ];
}
