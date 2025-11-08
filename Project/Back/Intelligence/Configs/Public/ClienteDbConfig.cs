using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class ClienteDbConfig : IEntityTypeConfiguration<Cliente>
{
    public const string ClienteIdSeq = "cliente_cliente_id_seq";

    public void Configure(EntityTypeBuilder<Cliente> entity)
    {
        entity.ToTable("cliente", "public");

        entity.HasKey(e => e.ClienteId)
            .HasName("cliente_pk");

        entity.Property(e => e.ClienteId)
            .ValueGeneratedOnAdd()
            .HasColumnName("cliente_id")
            .HasDefaultValueSql($"nextval('public.{ClienteIdSeq}')");

        entity.Property(e => e.AlteradoEm)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("alterado_em");

        entity.Property(e => e.AlteradoPor)
            .HasColumnName("alterado_por");

        entity.Property(e => e.ArmazenarPdfConsultas)
            .HasColumnName("armazenar_pdf_consultas");

        entity.Property(e => e.Ativo)
            .HasColumnName("ativo");

        entity.Property(e => e.Bairro)
            .HasMaxLength(50)
            .HasColumnName("bairro");

        entity.Property(e => e.BalanceInBrl)
            .HasPrecision(10, 2)
            .HasDefaultValueSql("0")
            .HasColumnName("balance_in_brl");

        entity.Property(e => e.BalanceType)
            .HasDefaultValue((short)2)
            .HasColumnName("balance_type");

        entity.Property(e => e.BalanceUpdatedAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("balance_updated_at");

        entity.Property(e => e.BillingId)
            .HasColumnName("billing_id");

        entity.Property(e => e.BillingInstructions)
            .HasColumnType("jsonb")
            .HasColumnName("billing_instructions");

        entity.Property(e => e.BlockSensitiveDataInQueryString)
            .HasColumnName("block_sensitive_data_in_query_string");

        entity.Property(e => e.Cep).HasColumnName("cep");

        entity.Property(e => e.Cidade)
            .HasMaxLength(100)
            .HasColumnName("cidade");

        entity.Property(e => e.ClienteEmTeste)
            .HasColumnName("cliente_em_teste");

        entity.Property(e => e.Comentarios)
            .HasMaxLength(1000)
            .HasColumnName("comentarios");

        entity.Property(e => e.Complemento)
            .HasMaxLength(30)
            .HasColumnName("complemento");

        entity.Property(e => e.CpfCnpj).HasColumnName("cpf_cnpj");

        entity.Property(e => e.CreditsLimitPerDay)
            .HasColumnName("credits_limit_per_day");

        entity.Property(e => e.CreditsLimitPerHour)
            .HasColumnName("credits_limit_per_hour");

        entity.Property(e => e.CreditsLimitPerMonth)
            .HasColumnName("credits_limit_per_month");

        entity.Property(e => e.CreditsLimitPerWeek)
            .HasColumnName("credits_limit_per_week");

        entity.Property(e => e.CrmClientId)
            .HasColumnName("crm_client_id");

        entity.Property(e => e.CurrencyLimitPerDay)
            .HasColumnName("currency_limit_per_day");

        entity.Property(e => e.CurrencyLimitPerHour)
            .HasColumnName("currency_limit_per_hour");

        entity.Property(e => e.CurrencyLimitPerMonth)
            .HasColumnName("currency_limit_per_month");

        entity.Property(e => e.CurrencyLimitPerWeek)
            .HasColumnName("currency_limit_per_week");

        entity.Property(e => e.DataAccessLevel)
            .HasDefaultValue((short)0)
            .HasColumnName("data_access_level");

        entity.Property(e => e.DoccheckUseEnrollmentByOrganization)
            .HasColumnName("doccheck_use_enrollment_by_organization");

        entity.Property(e => e.DossierIdToExecutePf)
            .HasDefaultValue(5040)
            .HasColumnName("dossier_id_to_execute_pf");

        entity.Property(e => e.DossierIdToExecutePfCreditAnalysis)
            .HasDefaultValue(5041)
            .HasColumnName("dossier_id_to_execute_pf_credit_analysis");

        entity.Property(e => e.DossierIdToExecutePj)
            .HasDefaultValue(5045)
            .HasColumnName("dossier_id_to_execute_pj");

        entity.Property(e => e.DossierIdToExecutePjCreditAnalysis)
            .HasDefaultValue(5046)
            .HasColumnName("dossier_id_to_execute_pj_credit_analysis");

        entity.Property(e => e.Endereco)
            .HasMaxLength(100)
            .HasColumnName("endereco");

        entity.Property(e => e.ExatoSalesContact)
            .HasColumnName("exato_sales_contact");

        entity.Property(e => e.ExcluidoEm)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("excluido_em");

        entity.Property(e => e.ExcluidoPor)
            .HasColumnName("excluido_por");

        entity.Property(e => e.ExternalDisplayName)
            .HasColumnName("external_display_name");

        entity.Property(e => e.ExternalId)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("external_id");

        entity.Property(e => e.FaturamentoTipoId)
            .HasColumnName("faturamento_tipo_id");

        entity.Property(e => e.GerarPdfConsultas)
            .HasColumnName("gerar_pdf_consultas");

        entity.Property(e => e.HabilitarConsultasPorEmail)
            .HasColumnName("habilitar_consultas_por_email");

        entity.Property(e => e.HighPerformance)
            .HasDefaultValue(false)
            .HasColumnName("high_performance");

        entity.Property(e => e.IncluidoEm)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("incluido_em");

        entity.Property(e => e.IncluidoPor)
            .HasColumnName("incluido_por");

        entity.Property(e => e.Interno)
            .HasColumnName("interno");

        entity.Property(e => e.InvoiceSlug)
            .HasColumnName("invoice_slug");

        entity.Property(e => e.IsBillingCustomer)
            .HasColumnName("is_billing_customer");

        entity.Property(e => e.LastCreditPurchaseDate)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("last_credit_purchase_date");

        entity.Property(e => e.MigratedAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("migrated_at");

        entity.Property(e => e.MigratedToClienteExternalId)
            .HasColumnName("migrated_to_cliente_external_id");

        entity.Property(e => e.Nome)
            .HasColumnType("citext")
            .HasColumnName("nome");

        entity.Property(e => e.NomeFantasiaRf)
            .HasColumnType("citext")
            .HasColumnName("nome_fantasia_rf");

        entity.Property(e => e.Numero)
            .HasMaxLength(10)
            .HasColumnName("numero");

        entity.Property(e => e.OrganizationSegmentId)
            .HasColumnName("organization_segment_id");

        entity.Property(e => e.Origem)
            .HasMaxLength(50)
            .HasColumnName("origem");

        entity.Property(e => e.Origin)
            .HasColumnName("origin");

        entity.Property(e => e.ParentOrganizationId)
            .HasColumnName("parent_organization_id");

        entity.Property(e => e.PartnerId)
            .HasColumnName("partner_id");

        entity.Property(e => e.PdfPassword)
            .HasColumnName("pdf_password");

        entity.Property(e => e.PessoaFisica)
            .HasColumnName("pessoa_fisica");

        entity.Property(e => e.PrecoId)
            .HasColumnName("preco_id");

        entity.Property(e => e.PublicId)
            .HasMaxLength(14)
            .IsFixedLength()
            .HasDefaultValueSql("generate_base36_id()")
            .HasColumnName("public_id");

        entity.Property(e => e.QuodCustomerExternalId)
            .HasColumnName("quod_customer_external_id");

        entity.Property(e => e.QuodLastEnrollment)
            .HasColumnType("jsonb")
            .HasColumnName("quod_last_enrollment");

        entity.Property(e => e.QuodSegmentId)
            .HasColumnName("quod_segment_id");

        entity.Property(e => e.QuodSuccessfullyEnrollmentAt)
            .HasColumnType("timestamp without time zone")
            .HasColumnName("quod_successfully_enrollment_at");

        entity.Property(e => e.RazaoSocialRf)
            .HasColumnType("citext")
            .HasColumnName("razao_social_rf");

        entity.Property(e => e.RealmId)
            .HasColumnName("realm_id");

        entity.Property(e => e.ReceitaCpfNeedPdfProof)
            .HasDefaultValue(true)
            .HasColumnName("receita_cpf_need_pdf_proof");

        entity.Property(e => e.ReceitaCpfShouldReturnMinor18AgeData)
            .HasDefaultValue(true)
            .HasColumnName("receita_cpf_should_return_minor18_age_data");

        entity.Property(e => e.ReceitaCpfUseSerproAsMainSource)
            .HasDefaultValue(false)
            .HasColumnName("receita_cpf_use_serpro_as_main_source");

        entity.Property(e => e.ResultDesatLimiteHrs)
            .HasColumnName("result_desat_limite_hrs");

        entity.Property(e => e.Saldo)
            .HasDefaultValue(0)
            .HasColumnName("saldo");

        entity.Property(e => e.SegundoNome)
            .HasMaxLength(100)
            .HasColumnName("segundo_nome");

        entity.Property(e => e.Slug)
            .HasColumnName("slug");

        entity.Property(e => e.StoreTransactionInput)
            .HasDefaultValue(true)
            .HasColumnName("store_transaction_input");

        entity.Property(e => e.StoreTransactionReturn)
            .HasDefaultValue(true)
            .HasColumnName("store_transaction_return");

        entity.Property(e => e.TransLimitPerDay)
            .HasColumnName("trans_limit_per_day");

        entity.Property(e => e.TransLimitPerHour)
            .HasColumnName("trans_limit_per_hour");

        entity.Property(e => e.TransLimitPerMonth)
            .HasColumnName("trans_limit_per_month");

        entity.Property(e => e.TransLimitPerWeek)
            .HasColumnName("trans_limit_per_week");

        entity.Property(e => e.Uf)
            .HasMaxLength(2)
            .IsFixedLength()
            .HasColumnName("uf");

        entity.Property(e => e.UnauthorizedDatasources)
            .HasColumnName("unauthorized_datasources");

        entity.Property(e => e.UseDoccheck)
            .HasColumnName("use_doccheck");

        entity.Property(e => e.UseOcrExato)
            .HasDefaultValue(false)
            .HasColumnName("use_ocr_exato");

        entity.Property(e => e.UseSerproDataValidFacial)
            .HasDefaultValue(false)
            .HasColumnName("use_serpro_data_valid_facial");

        entity.HasOne<FaturamentoTipo>()
            .WithMany()
            .HasPrincipalKey(f => f.FaturamentoTipoId)
            .HasForeignKey(e => e.FaturamentoTipoId)
            .HasConstraintName("Reffaturamento_tipo72");

        entity.HasOne<Realm>()
            .WithMany()
            .HasPrincipalKey(r => r.Id)
            .HasForeignKey(e => e.RealmId);

        entity.HasOne<OrganizationSegment>()
            .WithMany()
            .HasPrincipalKey(s => s.Id)
            .HasForeignKey(e => e.OrganizationSegmentId)
            .HasConstraintName("fk_organization_segment_id");

        entity.HasOne<Cliente>()
            .WithMany()
            .HasPrincipalKey(s => s.ClienteId)
            .HasForeignKey(e => e.ParentOrganizationId)
            .HasConstraintName("fk_parent_organization_id");

        entity.HasIndex(e => e.ClienteId)
            .IncludeProperties(e => new { e.ExternalId, e.Nome, e.CpfCnpj, e.PessoaFisica })
            .HasFilter("(parent_organization_id IS NULL)")
            .HasDatabaseName("cliente_ak_raiz")
            .IsUnique();

        entity.HasIndex(e => e.ExternalId)
            .HasDatabaseName("cliente_ak_external_id")
            .IsUnique();

        entity.Ignore(e => e.IsParent);
    }
}
