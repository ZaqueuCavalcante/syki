using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class UserPhoneNumberDbConfig : IEntityTypeConfiguration<UserPhoneNumber>
{
    public void Configure(EntityTypeBuilder<UserPhoneNumber> entity)
    {
        entity.ToTable("user_phone_numbers", "public");

        entity.HasKey(e => e.Id)
            .HasName("user_phone_numbers_pk");

        entity.Property(e => e.Id)
            .UseSerialColumn()
            .HasColumnName("id");

        entity.Property(e => e.AreaCode)
            .HasColumnName("area_code");

        entity.Property(e => e.ClienteId)
            .HasColumnName("cliente_id");

        entity.Property(e => e.CountryCode)
            .HasColumnName("country_code");

        entity.Property(e => e.MigratedAt)
            .HasColumnType("timestamp")
            .HasColumnName("migrated_at");

        entity.Property(e => e.MigratedFromClienteId)
            .HasColumnName("migrated_from_cliente_id");

        entity.Property(e => e.MigratedFromUserId)
            .HasColumnName("migrated_from_user_id");

        entity.Property(e => e.Number)
            .HasColumnName("number");

        entity.Property(e => e.OriginalInput)
            .HasColumnName("original_input");

        entity.Property(e => e.PhoneType)
            .HasColumnName("phone_type");

        entity.Property(e => e.UserId)
            .HasColumnName("user_id");

        entity.Property(e => e.VerificationDate)
            .HasColumnType("timestamp")
            .HasColumnName("verification_date");

        entity.Property(e => e.Verified)
            .HasColumnName("verified");

        entity.HasIndex(e => e.UserId)
            .HasDatabaseName("ix_user_phone_numbers_user_id");

        entity.HasIndex(e => new { e.CountryCode, e.Number })
            .HasDatabaseName("user_phone_numbers_country_number_idx")
            .IsUnique();

        entity.HasIndex(e => new { e.AreaCode, e.Number })
            .HasDatabaseName("user_phone_numbers_ddd_number_idx")
            .IsUnique();

        entity.HasOne<User>()
            .WithMany()
            .HasPrincipalKey(c => c.Id)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("user_phone_numbers_users_user_id_fkey");

        entity.HasOne<Cliente>()
            .WithMany()
            .HasPrincipalKey(c => c.ClienteId)
            .HasForeignKey(e => e.ClienteId);
    }
}
