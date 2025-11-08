namespace Exato.Back.Intelligence.Domain.Public;

public class Consulta
{
    public int ConsultaId { get; set; }

    public int? ConsultaMasterId { get; set; }

    public short ConsultaTipoId { get; set; }

    public int TokenAcessoId { get; set; }

    public short ConsultaResultadoTipoId { get; set; }

    public int? ConsultaReaproveitadaId { get; set; }

    public short OrigemId { get; set; }

    public short? ServidorId { get; set; }

    public short? IpSaidaId { get; set; }

    public int? ProxySaidaId { get; set; }

    public string UidBase36 { get; set; }

    public string? Caminho { get; set; }

    public int IpRemoto { get; set; }

    public bool AcessoNegado { get; set; }

    public DateTime Inicio { get; set; }

    public DateTime? Fim { get; set; }

    public string Chave { get; set; }

    public string Entrada { get; set; }

    public string? Retorno { get; set; }

    public int? RetornoTam { get; set; }

    public byte[]? RetornoSerializado { get; set; }

    public int? RetornoSerializadoTam { get; set; }

    public short? Tentativas { get; set; }

    public short? CaptchaRecusados { get; set; }

    public short? CaptchaFalhas { get; set; }

    public int? CaptchaTempoDecorridoMs { get; set; }

    public string? Mensagem { get; set; }

    public string? ErroDetalhe { get; set; }

    public int? ErroDetalheTam { get; set; }

    public byte[]? RetornoOriginalCompact { get; set; }

    public int? RetornoOriginalCompactTam { get; set; }

    public byte[]? PdfResultadoCompact { get; set; }

    public int? PdfResultadoCompactTam { get; set; }

    public bool Faturavel { get; set; }

    public bool ArmazenarPdf { get; set; }

    public short CustoCred { get; set; }

    public short CustoCredPdfGeracao { get; set; }

    public short CustoCredPdfArmaz { get; set; }

    public short CustoCredTotalIncSubcons { get; set; }

    public bool RetornoOriginalApagado { get; set; }

    public bool RetornoDesatualizado { get; set; }

    public short? QtdExecucoesParalelas { get; set; }

    public short? QtdSubConsultas { get; set; }

    public short? QtdSubConsultasArvore { get; set; }

    public short? QtdPdfArvore { get; set; }

    public string? Options { get; set; }

    public string? ValidationResults { get; set; }

    public string? Hostname { get; set; }

    public short? ValidationResultRiskIndicator { get; set; }

    public DateTime? Recencia { get; set; }

    public Guid? ExternalId { get; set; }

    public string? InputParams { get; set; }

    public string? Result { get; set; }

    public string? ResultData { get; set; }

    public long? RemoteIp { get; set; }

    public string? MasterUid { get; set; }

    public string? TransactionCid { get; set; }

    public bool? Async { get; set; }

    public bool? AsyncRunPersistent { get; set; }

    public short? AsyncAttempts { get; set; }

    public bool? AsyncChild { get; set; }

    public short? AsyncLastResultTypeId { get; set; }

    public string? AsyncLastMessage { get; set; }

    public string? AsyncLastError { get; set; }

    public DateTime? AsyncLastStart { get; set; }

    public DateTime? AsyncLastEnd { get; set; }

    public int? AsyncLastId { get; set; }

    public string? AsyncLastUid { get; set; }

    public decimal? CostInBrl { get; set; }

    public string? ResultProfilerJson { get; set; }

    public string? Document { get; set; }

    public string? StartInfoJson { get; set; }

    public Guid? CornerstoneApiCallInitBgcheckUid { get; set; }

    public short? PdfPasswordType { get; set; }

    public Guid? ChatbotProductRequestUid { get; set; }

    /// <summary>
    /// Indica se a consulta/transação é repetida no mesmo dia, com a mesma chave para a mesma organização.
    /// </summary>
    public bool? IsRepeatedOnSameDayKeyOrganization { get; set; }

    /// <summary>
    /// Id da ação que deve ser realizada com o campo que possui os bytes do PDF compactado.
    /// </summary>
    public short ResultPdfCompressedS3ActionId { get; set; }

    /// <summary>
    /// Data e hora em que o PDF compactado foi enviado para o S3. Caso seja null, não foi enviado.
    /// </summary>
    public DateTime? ResultPdfCompressedS3UploadedAt { get; set; }

    /// <summary>
    /// Id da ação que deve ser realizada com o campo que possui os bytes dos dados originais compactados.
    /// </summary>
    public short OriginalSourceDataCompressedS3ActionId { get; set; }

    /// <summary>
    /// Data e hora em que os dados originais compactados foram enviados para o S3. Caso seja null, não foi enviado.
    /// </summary>
    public DateTime? OriginalSourceDataCompressedS3UploadedAt { get; set; }

    /// <summary>
    /// Id do produto do fornecedor que foi usado nessa consulta/transação. Caso seja null, deve ser usado o campo da tabela de tipo de consulta.
    /// </summary>
    public short? OverriddenSupplierProductId { get; set; }

    /// <summary>
    /// Quantidade do produto do fornecedor que foi usado nessa consulta/transação. Caso seja null, deve ser usado o campo da tabela de tipo de consulta.
    /// </summary>
    public short? OverriddenSupplierProductQuantity { get; set; }

    public long? BatchRunId { get; set; }
}
