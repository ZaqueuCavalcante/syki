namespace Exato.Back.Intelligence.Domain.Public;

public class ConsultaTipo
{
    public short ConsultaTipoId { get; set; }

    public short? CaptchaMecanismoId { get; set; }

    public string Nome { get; set; }

    public string? Fonte { get; set; }

    public short QuantidadeMinSubconsultas { get; set; }

    public bool FaturarPelasSubconsultas { get; set; }

    public bool FaturarResultNaoEncontrado { get; set; }

    public short CustoCred { get; set; }

    public short CustoCredPdfGeracao { get; set; }

    public short CustoCredPdfArmaz { get; set; }

    public bool Visivel { get; set; }

    public bool Disponivel { get; set; }

    public string? MotivoIndisponivel { get; set; }

    public int? ClienteLimiteMs { get; set; }

    public bool? ProxyUsar { get; set; }

    public bool? ProxyForcar { get; set; }

    public bool? AlternarIpsEProxies { get; set; }

    public bool? BlockThreadsDuringIpProxySelection { get; set; }

    public bool? RandomIpProxySelection { get; set; }

    public short? BloqueioIpTimeoutQtd { get; set; }

    public int? BloqueioIpTimeoutTempoMs { get; set; }

    public short? BloqueioIpErroQtd { get; set; }

    public int? BloqueioIpErroTempoMs { get; set; }

    public short? BloqueioProxyTimeoutQtd { get; set; }

    public int? BloqueioProxyTimeoutTempoMs { get; set; }

    public short? BloqueioProxyErroQtd { get; set; }

    public int? BloqueioProxyErroTempoMs { get; set; }

    public int? GetTimeoutMs { get; set; }

    public int? CaptchaTimeoutMs { get; set; }

    public int? PostTimeoutMs { get; set; }

    public int? EsperaEntreCaptchaEPostMs { get; set; }

    public int? EsperaAntesNovaTentativaMs { get; set; }

    public short? MaxTentativas { get; set; }

    public short? DuracaoCacheHrs { get; set; }

    public short? MaxSimultaneasLote { get; set; }

    public short? MaxTentativasLote { get; set; }

    public string? EmailEntradaConsultas { get; set; }

    public bool? HabilitarLogDetalhado { get; set; }

    public bool? GerarPdf { get; set; }

    public bool? ArmazenarPdf { get; set; }

    public short? LimiteSimultApi { get; set; }

    public short? LimiteSimultPorClienteApi { get; set; }

    public short? QtdExecucoesParalelas { get; set; }

    public bool? PoolSessoesHabilitar { get; set; }

    public short? PoolSessoesTamanho { get; set; }

    public int? PoolSessoesTempoExpMs { get; set; }

    public short? PoolSessoesQtdProdutores { get; set; }

    public int? PoolSessoesIntervRefreshMs { get; set; }

    public bool? SessionPoolLastRequestUsingIp { get; set; }

    public bool? SessionPoolPersistUsingDatastore { get; set; }

    public bool? CaptchaOtimizarTaxaRecusados { get; set; }

    public short? OrdemExibicao { get; set; }

    public bool? ExibirNasListagens { get; set; }

    public DateTime AlteradoEm { get; set; }

    public int AlteradoPor { get; set; }

    public bool? AllowBatchProc { get; set; }

    public Guid? ExternalId { get; set; }

    public decimal? PriceInBrl { get; set; }

    public bool? IsDossier { get; set; }

    public long? OmieSourceId { get; set; }

    public bool? ExtSup { get; set; }

    public short? SupplierProductId { get; set; }

    /// <summary>
    /// Ver <see cref="ConsultaRelatorioTipo"/>
    /// </summary>
    public int? ConsultaRelatorioTipoId { get; set; }
}
