namespace Exato.Shared.Features.Office.CriarTokenDeAcesso;

public class CriarTokenDeAcessoOut : IApiDto<CriarTokenDeAcessoOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, CriarTokenDeAcessoOut)> GetExamples() =>
    [
        ("Exemplo", new CriarTokenDeAcessoOut() { }),
    ];
}
