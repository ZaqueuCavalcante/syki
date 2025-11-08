namespace Exato.Shared.Features.Office.BuscarTokensDeAcesso;

public class BuscarTokensDeAcessoIn : IApiDto<BuscarTokensDeAcessoIn>
{
    public int ClienteId { get; set; }

    public int Page { get; set; }
    public TokenAcessoKeyType? KeyType { get; set; }

    public static IEnumerable<(string, BuscarTokensDeAcessoIn)> GetExamples() =>
    [
        ("Exemplo", new BuscarTokensDeAcessoIn() { }),
    ];
}
