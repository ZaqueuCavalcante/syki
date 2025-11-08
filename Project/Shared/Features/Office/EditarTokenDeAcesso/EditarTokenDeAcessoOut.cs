namespace Exato.Shared.Features.Office.EditarTokenDeAcesso;

public class EditarTokenDeAcessoOut : IApiDto<EditarTokenDeAcessoOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, EditarTokenDeAcessoOut)> GetExamples() =>
    [
        ("Exemplo", new EditarTokenDeAcessoOut() { }),
    ];
}
