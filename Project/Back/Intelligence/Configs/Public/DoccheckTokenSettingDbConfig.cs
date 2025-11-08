using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class DoccheckTokenSettingDbConfig : IEntityTypeConfiguration<DoccheckTokenSetting>
{
    public void Configure(EntityTypeBuilder<DoccheckTokenSetting> entity)
    {
        entity.ToTable("doccheck_token_settings", "public");

        entity.HasKey(e => e.Id)
            .HasName("doccheck_token_settings_pkey");

        entity.Property(e => e.Id)
            .UseSerialColumn()
            .HasColumnName("id");

        entity.Property(e => e.ActionIfAgeNotInEstimatedRange)
            .HasColumnName("action_if_age_not_in_estimated_range");

        entity.Property(e => e.ActionIfBolsaFamilia)
            .HasColumnName("action_if_bolsa_familia");

        entity.Property(e => e.ActionIfCpfCancelled)
            .HasDefaultValueSql("'Reject'::text")
            .HasColumnName("action_if_cpf_cancelled");

        entity.Property(e => e.ActionIfDetectedFraud)
            .HasDefaultValueSql("'ManualValidation'::text")
            .HasColumnName("action_if_detected_fraud");

        entity.Property(e => e.ActionIfForeignDocument)
            .HasColumnName("action_if_foreign_document");

        entity.Property(e => e.ActionIfGoogleNewsOccurrences)
            .HasColumnName("action_if_google_news_occurrences");

        entity.Property(e => e.ActionIfLawsuitOccurrences)
            .HasColumnName("action_if_lawsuit_occurrences");

        entity.Property(e => e.ActionIfListedOnSanctions)
            .HasDefaultValueSql("'ManualValidation'::text")
            .HasColumnName("action_if_listed_on_sanctions");

        entity.Property(e => e.ActionIfMinor)
            .HasDefaultValueSql("'Reject'::text")
            .HasColumnName("action_if_minor");

        entity.Property(e => e.ActionIfNotOfficialDocument)
            .HasColumnName("action_if_not_official_document");

        entity.Property(e => e.ActionIfPep)
            .HasDefaultValueSql("'ManualValidation'::text")
            .HasColumnName("action_if_pep");

        entity.Property(e => e.ActionIfPreape)
            .HasDefaultValueSql("'Reject'::text")
            .HasColumnName("action_if_preape");

        entity.Property(e => e.ActionIfPreapeMatchCpf)
            .HasColumnName("action_if_preape_match_cpf");

        entity.Property(e => e.ActionIfPreapeMatchProbability08OrLess)
            .HasColumnName("action_if_preape_match_probability_08_or_less");

        entity.Property(e => e.ActionIfPreapeMatchProbability09)
            .HasColumnName("action_if_preape_match_probability_09");

        entity.Property(e => e.ActionIfValidationNotInBrazil)
            .HasColumnName("action_if_validation_not_in_brazil");

        entity.Property(e => e.AllowGlobalFaceComparison)
            .HasDefaultValue(true)
            .HasColumnName("allow_global_face_comparison");

        entity.Property(e => e.AwsLivenessPercentToApprove)
            .HasDefaultValueSql("70")
            .HasColumnName("aws_liveness_percent_to_approve");

        entity.Property(e => e.AwsLivenessPercentToReject)
            .HasColumnName("aws_liveness_percent_to_reject");

        entity.Property(e => e.AwsMatch3d3dSimilarityPercentToApprove)
            .HasColumnName("aws_match3d3d_similarity_percent_to_approve");

        entity.Property(e => e.AwsMatch3d3dSimilarityPercentToReject)
            .HasColumnName("aws_match3d3d_similarity_percent_to_reject");

        entity.Property(e => e.BaseAgeToConsiderMinor)
            .HasColumnName("base_age_to_consider_minor");

        entity.Property(e => e.CpfsCustomDomain)
            .HasColumnName("cpfs_custom_domain");

        entity.Property(e => e.CustomDomain)
            .HasColumnName("custom_domain");

        entity.Property(e => e.CustomShortDomain)
            .HasColumnName("custom_short_domain");

        entity.Property(e => e.CustomizationSettingsId)
            .HasColumnName("customization_settings_id");

        entity.Property(e => e.EnableJsPostMessage)
            .HasColumnName("enable_js_post_message");

        entity.Property(e => e.ForceManualAfterNReadDoc)
            .HasDefaultValue(5)
            .HasColumnName("force_manual_after_n_read_doc");

        entity.Property(e => e.ForceStatus)
            .HasColumnName("force_status");

        entity.Property(e => e.ForceStatusEnrollment)
            .HasColumnName("force_status_enrollment");

        entity.Property(e => e.ForceStatusNotEnrollment)
            .HasColumnName("force_status_not_enrollment");

        entity.Property(e => e.KeyId)
            .HasColumnName("key_id");

        entity.Property(e => e.MaxMatchLevelForManualReview)
            .HasColumnName("max_match_level_for_manual_review");

        entity.Property(e => e.MaxMatchLevelForRejection)
            .HasColumnName("max_match_level_for_rejection");

        entity.Property(e => e.MinMatchLevel)
            .HasColumnName("min_match_level");

        entity.Property(e => e.MinutesToExpireValidationLink)
            .HasColumnName("minutes_to_expire_validation_link");

        entity.Property(e => e.NumberAttemptsMatch3d3d)
            .HasDefaultValue(5)
            .HasColumnName("number_attempts_match3d3d");

        entity.Property(e => e.OcrSupplier)
            .HasColumnName("ocr_supplier");

        entity.Property(e => e.RedirectToDigitalDoc)
            .HasDefaultValue(false)
            .HasColumnName("redirect_to_digital_doc");

        entity.Property(e => e.RedirectUrl)
            .HasColumnName("redirect_url");

        entity.Property(e => e.RejectedValidationToManualReview)
            .HasDefaultValue(false)
            .HasColumnName("rejected_validation_to_manual_review");

        entity.Property(e => e.ReplayThrowError)
            .HasDefaultValue(false)
            .HasColumnName("replay_throw_error");

        entity.Property(e => e.SilentFlow)
            .HasColumnName("silent_flow");

        entity.Property(e => e.StatusWhenMinMatchNotMet)
            .HasColumnName("status_when_min_match_not_met");

        entity.Property(e => e.UseAiToDetectTextsFromDocument)
            .HasColumnName("use_ai_to_detect_texts_from_document");

        entity.Property(e => e.UseAwsToMatch3d3d)
            .HasDefaultValue(true)
            .HasColumnName("use_aws_to_match3d3d");

        entity.Property(e => e.UseExatoToken)
            .HasDefaultValue(false)
            .HasColumnName("use_exato_token");

        entity.Property(e => e.UseOcrAwsFallback)
            .HasDefaultValue(true)
            .HasColumnName("use_ocr_aws_fallback");

        entity.Property(e => e.ValidationLimitPerMonth)
            .HasColumnName("validation_limit_per_month");

        entity.Property(e => e.WebhookIncludeDetailsAml)
            .HasColumnName("webhook_include_details_aml");

        entity.Property(e => e.WebhookUrl)
            .HasColumnName("webhook_url");

        entity.HasIndex(e => e.KeyId)
            .HasDatabaseName("doccheck_token_settings_ie_key_id")
            .IsUnique();
    }
}
