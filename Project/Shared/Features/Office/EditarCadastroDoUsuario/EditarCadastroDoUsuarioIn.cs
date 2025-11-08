namespace Exato.Shared.Features.Office.EditarCadastroDoUsuario;

public class EditarCadastroDoUsuarioIn : IApiDto<EditarCadastroDoUsuarioIn>
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string? Cpf { get; set; }

    public static IEnumerable<(string, EditarCadastroDoUsuarioIn)> GetExamples() =>
    [
        ("Exemplo", new EditarCadastroDoUsuarioIn() { }),
    ];
}
