using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class UserDbConfig : IEntityTypeConfiguration<User>
{
    public const string UsersIdSeq = "users_id_seq";

    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.ToTable("users", "public");

        entity.HasKey(e => e.Id)
            .HasName("users_pkey");

        entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id")
            .HasDefaultValueSql($"nextval('public.{UsersIdSeq}')");

        entity.Property(e => e.Active)
            .HasColumnName("active");

        entity.Property(e => e.ClienteId)
            .HasColumnName("cliente_id");

        entity.Property(e => e.Cpf)
            .HasColumnName("cpf");

        entity.Property(e => e.CreatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("created_at");

        entity.Property(e => e.CreatedBy)
            .HasDefaultValue(0)
            .HasColumnName("created_by");

        entity.Property(e => e.CurrentSignInAt)
            .HasColumnType("timestamp")
            .HasColumnName("current_sign_in_at");

        entity.Property(e => e.CurrentSignInIp)
            .HasColumnName("current_sign_in_ip");

        entity.Property(e => e.DeletedAt)
            .HasColumnType("timestamp")
            .HasColumnName("deleted_at");

        entity.Property(e => e.DeletedBy)
            .HasColumnName("deleted_by");

        entity.Property(e => e.Email)
            .HasDefaultValueSql("''")
            .HasColumnType("varchar")
            .HasColumnName("email");

        entity.Property(e => e.EncryptedPassword)
            .HasDefaultValueSql("''")
            .HasColumnType("varchar")
            .HasColumnName("encrypted_password");

        entity.Property(e => e.ExternalId)
            .HasColumnName("external_id");

        entity.Property(e => e.FullName)
            .HasMaxLength(70)
            .HasColumnName("full_name");

        entity.Property(e => e.Internal)
            .HasColumnName("internal");

        entity.Property(e => e.InvitationAcceptedAt)
            .HasColumnType("timestamp")
            .HasColumnName("invitation_accepted_at");

        entity.Property(e => e.InvitationCreatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("invitation_created_at");

        entity.Property(e => e.InvitationLimit)
            .HasColumnName("invitation_limit");

        entity.Property(e => e.InvitationSentAt)
            .HasColumnType("timestamp")
            .HasColumnName("invitation_sent_at");

        entity.Property(e => e.InvitationToken)
            .HasColumnType("varchar")
            .HasColumnName("invitation_token");

        entity.Property(e => e.InvitationsCount)
            .HasDefaultValue(0)
            .HasColumnName("invitations_count");

        entity.Property(e => e.InvitedById)
            .HasColumnName("invited_by_id");

        entity.Property(e => e.InvitedByType)
            .HasColumnType("varchar")
            .HasColumnName("invited_by_type");

        entity.Property(e => e.LastSignInAt)
            .HasColumnType("timestamp")
            .HasColumnName("last_sign_in_at");

        entity.Property(e => e.LastSignInIp)
            .HasColumnName("last_sign_in_ip");

        entity.Property(e => e.MigratedAt)
            .HasColumnName("migrated_at");

        entity.Property(e => e.MigratedToUserExternalId)
            .HasColumnName("migrated_to_user_external_id");

        entity.Property(e => e.OrigemId)
            .HasColumnName("origem_id");

        entity.Property(e => e.Origin)
            .HasColumnName("origin");

        entity.Property(e => e.Provider)
            .HasColumnType("varchar")
            .HasColumnName("provider");

        entity.Property(e => e.RealmId)
            .HasColumnName("realm_id");

        entity.Property(e => e.RememberCreatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("remember_created_at");

        entity.Property(e => e.ResetPasswordSentAt)
            .HasColumnType("timestamp")
            .HasColumnName("reset_password_sent_at");

        entity.Property(e => e.ResetPasswordToken)
            .HasColumnType("varchar")
            .HasColumnName("reset_password_token");

        entity.Property(e => e.SignInCount)
            .HasDefaultValue(0)
            .HasColumnName("sign_in_count");

        entity.Property(e => e.Uid)
            .HasColumnType("varchar")
            .HasColumnName("uid");

        entity.Property(e => e.UpdatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("updated_at");

        entity.Property(e => e.UpdatedBy)
            .HasColumnName("updated_by");

        entity.Property(e => e.Visible)
            .HasColumnName("visible");
    }
}
