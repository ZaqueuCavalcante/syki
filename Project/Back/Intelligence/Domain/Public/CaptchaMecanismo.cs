namespace Exato.Back.Intelligence.Domain.Public;

public class CaptchaMecanismo
{
    public short CaptchaMecanismoId { get; set; }

    public string Nome { get; set; }

    public string? Host { get; set; }

    public int? Port { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public int? TimeoutMs { get; set; }

    public bool Ativo { get; set; }

    public int UtilizacaoQuantidade { get; set; }

    public DateTime? UtilizacaoUltimoData { get; set; }

    public int ResolvidoQuantidade { get; set; }

    public DateTime? ResolvidoUltimoData { get; set; }

    public int SucessoQuantidade { get; set; }

    public int IncorretoQuantidade { get; set; }

    public int NaoResolvidoQuantidade { get; set; }

    public DateTime? NaoResolvidoUltimoData { get; set; }

    public int? TempoMedioSolucaoCorretaMs { get; set; }

    public DateTime AlteradoEm { get; set; }

    public int AlteradoPor { get; set; }

    public long? OmieSourceId { get; set; }
}
