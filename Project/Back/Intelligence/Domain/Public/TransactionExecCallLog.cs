using System.Net;

namespace Exato.Back.Intelligence.Domain.Public;

public class TransactionExecCallLog
{
    public int TransactionExecCallLogId { get; set; }

    public Guid TransactionExecCallLogUid { get; set; }

    public DateTime InsertedOnUtc { get; set; }

    public string TransactionUid { get; set; }

    public string? TransactionCid { get; set; }

    public string? OperationMethod { get; set; }

    public string? ExecutionMode { get; set; }

    public int? DataSourceTypeId { get; set; }

    public string? DataSourceTypeEnum { get; set; }

    public int? IdentityId { get; set; }

    public Guid? IdentityKeyId { get; set; }

    public string? IdentityTokenStr { get; set; }

    public IPAddress? RemoteAddr { get; set; }

    public string? ServerInstance { get; set; }

    public string? LoadBalancerTraceHeader { get; set; }

    public string? LoadBalancerTraceField { get; set; }

    public string? LoadBalancerTraceId { get; set; }

    public string? HttpMethod { get; set; }

    public string? HttpUri { get; set; }

    public string? HttpHost { get; set; }

    public string[]? HttpHeaders { get; set; }

    public string? HttpBodyText { get; set; }

    public byte[]? HttpBodyRaw { get; set; }

    public string? EventCountersInfo { get; set; }

    public string? AdditionalInfo { get; set; }
}
