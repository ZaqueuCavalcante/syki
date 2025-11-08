using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class ConsultaDbConfig : IEntityTypeConfiguration<Consulta>
{
    public const string ConsultaIdSeq = "consulta_consulta_id_seq";

    public void Configure(EntityTypeBuilder<Consulta> entity)
    {
        entity.ToTable("consulta", "public", x =>
        {
            x.HasCheckConstraint("consulta_uid_base36_check", "char_length((uid_base36)::text) <= 25");
        })
        .HasAnnotation("Npgsql:StorageParameter:autovacuum_enabled", "true");

        entity.HasKey(e => e.ConsultaId)
            .HasName("consulta_pk");

        entity.Property(e => e.ConsultaId)
            .ValueGeneratedOnAdd()
            .HasColumnName("consulta_id")
            .HasDefaultValueSql($"nextval('public.{ConsultaIdSeq}')");

        entity.Property(e => e.ConsultaMasterId)
            .HasColumnName("consulta_master_id");

        entity.Property(e => e.ConsultaTipoId)
            .HasColumnName("consulta_tipo_id");

        entity.Property(e => e.TokenAcessoId)
            .HasColumnName("token_acesso_id");

        entity.Property(e => e.ConsultaResultadoTipoId)
            .HasColumnName("consulta_resultado_tipo_id");

        entity.Property(e => e.ConsultaReaproveitadaId)
            .HasColumnName("consulta_reaproveitada_id");

        entity.Property(e => e.OrigemId)
            .HasColumnName("origem_id");

        entity.Property(e => e.ServidorId)
            .HasColumnName("servidor_id");

        entity.Property(e => e.IpSaidaId)
            .HasColumnName("ip_saida_id");

        entity.Property(e => e.ProxySaidaId)
            .HasColumnName("proxy_saida_id");

        entity.Property(e => e.UidBase36)
            .UseCollation("C")
            .HasColumnType("citext")
            .HasColumnName("uid_base36");

        entity.Property(e => e.Caminho)
            .HasColumnName("caminho");

        entity.Property(e => e.IpRemoto)
            .HasColumnName("ip_remoto");

        entity.Property(e => e.AcessoNegado)
            .HasColumnName("acesso_negado");

        entity.Property(e => e.Inicio)
            .HasColumnType("timestamp")
            .HasColumnName("inicio");

        entity.Property(e => e.Fim)
            .HasColumnType("timestamp")
            .HasColumnName("fim");

        entity.Property(e => e.Chave)
            .UseCollation("C")
            .HasColumnType("citext")
            .HasColumnName("chave");

        entity.Property(e => e.Entrada)
            .HasMaxLength(1000)
            .HasColumnName("entrada");

        entity.Property(e => e.Retorno)
            .HasColumnName("retorno");

        entity.Property(e => e.RetornoTam)
            .HasColumnName("retorno_tam");

        entity.Property(e => e.RetornoSerializado)
            .HasColumnName("retorno_serializado");

        entity.Property(e => e.RetornoSerializadoTam)
            .HasColumnName("retorno_serializado_tam");

        entity.Property(e => e.Tentativas)
            .HasColumnName("tentativas");

        entity.Property(e => e.CaptchaRecusados)
            .HasColumnName("captcha_recusados");

        entity.Property(e => e.CaptchaFalhas)
            .HasColumnName("captcha_falhas");

        entity.Property(e => e.CaptchaTempoDecorridoMs)
            .HasColumnName("captcha_tempo_decorrido_ms");

        entity.Property(e => e.Mensagem)
            .HasMaxLength(200)
            .HasColumnName("mensagem");

        entity.Property(e => e.ErroDetalhe)
            .HasColumnName("erro_detalhe");

        entity.Property(e => e.ErroDetalheTam)
            .HasColumnName("erro_detalhe_tam");

        entity.Property(e => e.RetornoOriginalCompact)
            .HasColumnName("retorno_original_compact");

        entity.Property(e => e.RetornoOriginalCompactTam)
            .HasColumnName("retorno_original_compact_tam");

        entity.Property(e => e.PdfResultadoCompact)
            .HasColumnName("pdf_resultado_compact");

        entity.Property(e => e.PdfResultadoCompactTam)
            .HasColumnName("pdf_resultado_compact_tam");

        entity.Property(e => e.Faturavel)
            .HasColumnName("faturavel");

        entity.Property(e => e.ArmazenarPdf)
            .HasColumnName("armazenar_pdf");

        entity.Property(e => e.CustoCred)
            .HasDefaultValue((short)0)
            .HasColumnName("custo_cred");

        entity.Property(e => e.CustoCredPdfGeracao)
            .HasDefaultValue((short)0)
            .HasColumnName("custo_cred_pdf_geracao");

        entity.Property(e => e.CustoCredPdfArmaz)
            .HasDefaultValue((short)0)
            .HasColumnName("custo_cred_pdf_armaz");

        entity.Property(e => e.CustoCredTotalIncSubcons)
            .HasDefaultValue((short)0)
            .HasColumnName("custo_cred_total_inc_subcons");

        entity.Property(e => e.RetornoOriginalApagado)
            .HasColumnName("retorno_original_apagado");

        entity.Property(e => e.RetornoDesatualizado)
            .HasColumnName("retorno_desatualizado");

        entity.Property(e => e.QtdExecucoesParalelas)
            .HasColumnName("qtd_execucoes_paralelas");

        entity.Property(e => e.QtdSubConsultas)
            .HasColumnName("qtd_sub_consultas");

        entity.Property(e => e.QtdSubConsultasArvore)
            .HasColumnName("qtd_sub_consultas_arvore");

        entity.Property(e => e.QtdPdfArvore)
            .HasColumnName("qtd_pdf_arvore");

        entity.Property(e => e.Options)
            .HasColumnName("options");

        entity.Property(e => e.ValidationResults)
            .HasColumnName("validation_results");

        entity.Property(e => e.Hostname)
            .HasMaxLength(50)
            .HasColumnName("hostname");

        entity.Property(e => e.ValidationResultRiskIndicator)
            .HasColumnName("validation_result_risk_indicator");

        entity.Property(e => e.Recencia)
            .HasColumnType("timestamp")
            .HasColumnName("recencia");

        entity.Property(e => e.ExternalId)
            .HasColumnName("external_id");

        entity.Property(e => e.InputParams)
            .HasColumnType("jsonb")
            .HasColumnName("input_params");

        entity.Property(e => e.Result)
            .HasColumnType("jsonb")
            .HasColumnName("result");

        entity.Property(e => e.ResultData)
            .HasColumnType("jsonb")
            .HasColumnName("result_data");

        entity.Property(e => e.RemoteIp)
            .HasColumnName("remote_ip");

        entity.Property(e => e.MasterUid)
            .HasColumnType("citext")
            .HasColumnName("master_uid");

        entity.Property(e => e.TransactionCid)
            .HasColumnType("citext")
            .HasColumnName("transaction_cid");

        entity.Property(e => e.Async)
            .HasColumnName("async");

        entity.Property(e => e.AsyncRunPersistent)
            .HasColumnName("async_run_persistent");

        entity.Property(e => e.AsyncAttempts)
            .HasColumnName("async_attempts");

        entity.Property(e => e.AsyncChild)
            .HasColumnName("async_child");

        entity.Property(e => e.AsyncLastResultTypeId)
            .HasColumnName("async_last_result_type_id");

        entity.Property(e => e.AsyncLastMessage)
            .HasColumnName("async_last_message");

        entity.Property(e => e.AsyncLastError)
            .HasColumnName("async_last_error");

        entity.Property(e => e.AsyncLastStart)
            .HasColumnType("timestamp")
            .HasColumnName("async_last_start");

        entity.Property(e => e.AsyncLastEnd)
            .HasColumnType("timestamp")
            .HasColumnName("async_last_end");

        entity.Property(e => e.AsyncLastId)
            .HasColumnName("async_last_id");

        entity.Property(e => e.AsyncLastUid)
            .HasColumnType("citext")
            .HasColumnName("async_last_uid");

        entity.Property(e => e.CostInBrl)
            .HasColumnType("money")
            .HasColumnName("cost_in_brl");

        entity.Property(e => e.ResultProfilerJson)
            .HasColumnType("jsonb")
            .HasColumnName("result_profiler_json");

        entity.Property(e => e.Document)
            .UseCollation("C")
            .HasColumnType("citext")
            .HasColumnName("document");

        entity.Property(e => e.StartInfoJson)
            .HasColumnType("jsonb")
            .HasColumnName("start_info_json");

        entity.Property(e => e.CornerstoneApiCallInitBgcheckUid)
            .HasColumnName("cornerstone_api_call_init_bgcheck_uid");

        entity.Property(e => e.PdfPasswordType)
            .HasColumnName("pdf_password_type");

        entity.Property(e => e.ChatbotProductRequestUid)
            .HasColumnName("chatbot_product_request_uid");

        entity.Property(e => e.IsRepeatedOnSameDayKeyOrganization)
            .HasColumnName("is_repeated_on_same_day_key_organization");

        entity.Property(e => e.ResultPdfCompressedS3ActionId)
            .HasDefaultValue((short)0)
            .HasColumnName("result_pdf_compressed_s3_action_id");

        entity.Property(e => e.ResultPdfCompressedS3UploadedAt)
            .HasColumnName("result_pdf_compressed_s3_uploaded_at");

        entity.Property(e => e.OriginalSourceDataCompressedS3ActionId)
            .HasDefaultValue((short)0)
            .HasColumnName("original_source_data_compressed_s3_action_id");

        entity.Property(e => e.OriginalSourceDataCompressedS3UploadedAt)
            .HasColumnName("original_source_data_compressed_s3_uploaded_at");

        entity.Property(e => e.OverriddenSupplierProductId)
            .HasColumnName("overridden_supplier_product_id");

        entity.Property(e => e.OverriddenSupplierProductQuantity)
            .HasColumnName("overridden_supplier_product_quantity");

        entity.Property(e => e.BatchRunId)
            .HasColumnName("batch_run_id");

        entity.HasIndex(e => e.TransactionCid, "consulta_ak_transaction_cid")
            .HasFilter("(transaction_cid IS NOT NULL)")
            .IsUnique();

        entity.HasIndex(e => e.UidBase36, "consulta_ak_uid")
            .IncludeProperties(e => e.ConsultaId)
            .UseCollation("C")
            .IsUnique();
    }
}
