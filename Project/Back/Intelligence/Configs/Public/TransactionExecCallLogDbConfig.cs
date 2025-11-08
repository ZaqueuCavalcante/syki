using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class TransactionExecCallLogDbConfig : IEntityTypeConfiguration<TransactionExecCallLog>
{
    public const string TransactionExecCallLogIdSeq = "transaction_exec_call_log_transaction_exec_call_log_id_seq";

    public void Configure(EntityTypeBuilder<TransactionExecCallLog> entity)
    {
        entity.ToTable("transaction_exec_call_log", "public");

        entity.HasKey(e => new { e.TransactionExecCallLogId, e.InsertedOnUtc })
            .HasName("transaction_exec_call_log_pk");

        entity.Property(e => e.TransactionExecCallLogId)
            .ValueGeneratedOnAdd()
            .HasColumnName("transaction_exec_call_log_id")
            .HasDefaultValueSql($"nextval('public.{TransactionExecCallLogIdSeq}')");

        entity.Property(e => e.InsertedOnUtc)
            .HasColumnType("timestamp with time zone")
            .HasColumnName("inserted_on_utc");

        entity.Property(e => e.AdditionalInfo)
            .HasColumnType("jsonb")
            .HasColumnName("additional_info");

        entity.Property(e => e.DataSourceTypeEnum)
            .HasColumnName("data_source_type_enum");

        entity.Property(e => e.DataSourceTypeId)
            .HasColumnName("data_source_type_id");

        entity.Property(e => e.EventCountersInfo)
            .HasColumnType("jsonb")
            .HasColumnName("event_counters_info");

        entity.Property(e => e.ExecutionMode)
            .HasColumnType("citext")
            .HasColumnName("execution_mode");

        entity.Property(e => e.HttpBodyRaw)
            .HasColumnName("http_body_raw");

        entity.Property(e => e.HttpBodyText)
            .HasColumnName("http_body_text");

        entity.Property(e => e.HttpHeaders)
            .HasColumnName("http_headers");

        entity.Property(e => e.HttpHost)
            .HasColumnName("http_host");

        entity.Property(e => e.HttpMethod)
            .HasColumnType("citext")
            .HasColumnName("http_method");

        entity.Property(e => e.HttpUri)
            .HasColumnName("http_uri");

        entity.Property(e => e.IdentityId)
            .HasColumnName("identity_id");

        entity.Property(e => e.IdentityKeyId)
            .HasColumnName("identity_key_id");

        entity.Property(e => e.IdentityTokenStr)
            .HasColumnType("citext")
            .HasColumnName("identity_token_str");

        entity.Property(e => e.LoadBalancerTraceField)
            .HasColumnName("load_balancer_trace_field");

        entity.Property(e => e.LoadBalancerTraceHeader)
            .HasColumnName("load_balancer_trace_header");

        entity.Property(e => e.LoadBalancerTraceId)
            .HasColumnType("citext")
            .HasColumnName("load_balancer_trace_id");

        entity.Property(e => e.OperationMethod)
            .HasColumnType("citext")
            .HasColumnName("operation_method");

        entity.Property(e => e.RemoteAddr)
            .HasColumnName("remote_addr");

        entity.Property(e => e.ServerInstance)
            .HasColumnType("citext")
            .HasColumnName("server_instance");

        entity.Property(e => e.TransactionCid)
            .HasColumnType("citext")
            .HasColumnName("transaction_cid");

        entity.Property(e => e.TransactionExecCallLogUid)
            .HasColumnName("transaction_exec_call_log_uid");

        entity.Property(e => e.TransactionUid)
            .HasColumnType("citext")
            .HasColumnName("transaction_uid");
    }
}
