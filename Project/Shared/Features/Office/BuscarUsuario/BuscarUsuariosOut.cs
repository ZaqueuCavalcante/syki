namespace Exato.Shared.Features.Office.BuscarUsuario;

public class BuscarUsuarioOut : IApiDto<BuscarUsuarioOut>
{
    public int Id { get; set; }
    public int WebUserId { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string Documento { get; set; }
    public bool Ativo { get; set; }
    public DateTime CriadoEm { get; set; }
    public ExatoWebOnboardStatus? OnboardStatus { get; set; }
    public DateTime? DeletedAt { get; set; }

    public int ClienteId { get; set; }
    public int Creditos { get; set; }
    public decimal BalanceInBrl { get; set; }
    public BalanceType BalanceType { get; set; }
    public MetodoDePagamento MetodoDePagamento { get; set; }

    public List<ExatoWebClaims> Claims { get; set; } = [];

    public int DexterClienteId { get; set; }
    public string? DexterCliente { get; set; }

    public string GetDexterCliente()
    {
        var sufix = DexterClienteId == ClienteId ? " (*empresa do próprio usuário)" : "";

        return $"{DexterCliente}{sufix}";
    }

    public static IEnumerable<(string, BuscarUsuarioOut)> GetExamples() =>
    [
        ("Exemplo", new BuscarUsuarioOut() { }),
    ];
}
