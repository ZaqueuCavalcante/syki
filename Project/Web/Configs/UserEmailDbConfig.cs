using Exato.Web.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Web.Configs;

public class UserEmailDbConfig : IEntityTypeConfiguration<UserEmail>
{
    public void Configure(EntityTypeBuilder<UserEmail> entity)
    {
        entity.ToTable("user_emails", "public");

        entity.HasKey(e => e.Id)
            .HasName("pk_user_emails");

        entity.Property(e => e.Id)
            .UseIdentityColumn()
            .HasColumnName("id");

        entity.Property(e => e.DeletedAt)
            .HasColumnType("timestamp")
            .HasColumnName("deleted_at");

        entity.Property(e => e.DeletedBy)
            .HasColumnName("deleted_by");

        entity.Property(e => e.Email)
            .HasColumnName("email");

        entity.Property(e => e.Main)
            .HasColumnName("main");

        entity.Property(e => e.UserId)
            .HasColumnName("user_id");

        entity.Property(e => e.VerificationDate)
            .HasColumnType("timestamp")
            .HasColumnName("verification_date");

        entity.Property(e => e.Verified)
            .HasColumnName("verified");

        entity.HasOne<WebUser>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.UserId)
            .HasConstraintName("fk_user_emails_users_user_id");

        entity.HasIndex(e => e.UserId)
            .HasDatabaseName("ix_user_emails_user_id");

        entity.HasIndex(e => new { e.Email, e.DeletedAt })
            .HasDatabaseName("ix_user_emails_email")
            .IsUnique();

        entity.HasIndex(e => new { e.Email, e.Verified })
            .HasDatabaseName("user_email_verified_idx")
            .IsUnique();
    }
}
