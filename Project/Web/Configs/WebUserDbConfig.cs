using Exato.Web.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Web.Configs;

public class WebUserDbConfig : IEntityTypeConfiguration<WebUser>
{
    public void Configure(EntityTypeBuilder<WebUser> entity)
    {
        entity.ToTable("users", "public");

        entity.HasKey(e => e.Id)
            .HasName("pk_users");

        entity.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        entity.Property(e => e.Active)
            .HasDefaultValue(true)
            .HasColumnName("active");

        entity.Property(e => e.AddressId)
            .HasColumnName("address_id");

        entity.Property(e => e.Birthday)
            .HasColumnType("timestamp")
            .HasColumnName("birthday");

        entity.Property(e => e.BonusForAddingPhone)
            .HasDefaultValue(false)
            .HasColumnName("bonus_for_adding_phone");

        entity.Property(e => e.BonusForIndicatorGiven)
            .HasColumnName("bonus_for_indicator_given");

        entity.Property(e => e.CameFromChatbot)
            .HasColumnName("came_from_chatbot");

        entity.Property(e => e.CameFromRegisterPostPaid)
            .HasColumnName("came_from_register_post_paid");

        entity.Property(e => e.Comments)
            .HasColumnName("comments");

        entity.Property(e => e.Cpf)
            .HasMaxLength(11)
            .HasColumnName("cpf");

        entity.Property(e => e.CreationDate)
            .HasColumnType("timestamp")
            .HasColumnName("creation_date");

        entity.Property(e => e.EmailMain)
            .HasColumnType("character varying")
            .HasColumnName("email_main");

        entity.Property(e => e.ExtraClaims)
            .HasColumnName("extra_claims");

        entity.Property(e => e.IndicatedByUserCompanyId)
            .HasColumnName("indicated_by_user_company_id");

        entity.Property(e => e.LastAccessIp)
            .HasMaxLength(45)
            .HasColumnName("last_access_ip");

        entity.Property(e => e.LastImpersonatedCompanyUid)
            .HasColumnName("last_impersonated_company_uid");

        entity.Property(e => e.LastPasswordUpdateDate)
            .HasColumnType("timestamp")
            .HasColumnName("last_password_update_date");

        entity.Property(e => e.MotherName)
            .HasMaxLength(100)
            .HasColumnName("mother_name");

        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .HasColumnName("name");

        entity.Property(e => e.OnboardCompletedWithoutValidation)
            .HasDefaultValue(false)
            .HasColumnName("onboard_completed_without_validation");

        entity.Property(e => e.OnboardStatus)
            .HasColumnName("onboard_status");

        entity.Property(e => e.PartnerKeyId)
            .HasColumnName("partner_key_id");

        entity.Property(e => e.Password)
            .HasMaxLength(100)
            .HasColumnName("password");

        entity.Property(e => e.PasswordAlgorithm)
            .HasMaxLength(100)
            .HasColumnName("password_algorithm");

        entity.Property(e => e.PasswordSeed)
            .HasMaxLength(80)
            .HasColumnName("password_seed");

        entity.Property(e => e.PaymentMode)
            .HasColumnName("payment_mode");

        entity.Property(e => e.PictureFilePath)
            .HasMaxLength(60)
            .HasColumnName("picture_file_path");

        entity.Property(e => e.PromotionalCodeId)
            .HasColumnName("promotional_code_id");

        entity.Property(e => e.RetryAttemptsQuiz)
            .HasColumnName("retry_attempts_quiz");

        entity.Property(e => e.SoftDelete)
            .HasColumnName("soft_delete");

        entity.Property(e => e.TwoFactorRequiredAt)
            .HasColumnType("timestamp")
            .HasColumnName("two_factor_required_at");

        entity.Property(e => e.UserUid)
            .HasMaxLength(36)
            .HasColumnName("user_uid");

        entity.Property(e => e.WrongPasswordAttempts)
            .HasColumnName("wrong_password_attempts");

        entity.HasIndex(e => e.UserUid, "ix_users_user_uid")
            .IsUnique();
    }
}
