namespace Exato.Shared.Features.Office.CriarUsuario;

public class CriarUsuarioIn : IApiDto<CriarUsuarioIn>
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string? Cpf { get; set; }
    public List<ExatoWebClaims> Claims { get; set; }

    public static IEnumerable<(string, CriarUsuarioIn)> GetExamples() =>
    [
        ("Exemplo", new CriarUsuarioIn() { }),
    ];
}
