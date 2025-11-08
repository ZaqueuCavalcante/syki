using Exato.Web.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Web.Configs;

public class UserPhoneNumberDbConfig : IEntityTypeConfiguration<UserPhoneNumber>
{
    public void Configure(EntityTypeBuilder<UserPhoneNumber> entity)
    {
        entity.ToTable("user_phone_numbers", "public");

        entity.HasKey(e => e.Id)
            .HasName("pk_user_phone_numbers");

        entity.Property(e => e.Id)
            .UseIdentityColumn()
            .HasColumnName("id");

        entity.Property(e => e.CompanyId)
            .HasColumnName("company_id");

        entity.Property(e => e.Ddd)
            .HasMaxLength(2)
            .HasColumnName("ddd");

        entity.Property(e => e.Main)
            .HasColumnName("main");

        entity.Property(e => e.Number)
            .HasMaxLength(9)
            .HasColumnName("number");

        entity.Property(e => e.Type)
            .HasColumnName("type");

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
            .HasConstraintName("fk_user_phone_numbers_users_user_id");

        entity.HasOne<Company>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.CompanyId)
            .HasConstraintName("fk_user_phone_numbers_companies_company_id");

        entity.HasIndex(e => new { e.Ddd, e.Number })
            .HasDatabaseName("ix_user_phone_numbers_ddd_number")
            .IsUnique();
    }
}
