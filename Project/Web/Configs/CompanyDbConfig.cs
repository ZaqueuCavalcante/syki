using Exato.Web.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Web.Configs;

public class CompanyDbConfig : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> entity)
    {
        entity.ToTable("companies", "public");

        entity.HasKey(e => e.Id)
            .HasName("pk_companies");

        entity.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        entity.Property(e => e.Active)
            .HasColumnName("active");

        entity.Property(e => e.AddressCommercialId)
            .HasColumnName("address_commercial_id");

        entity.Property(e => e.AddressFiscalId)
            .HasColumnName("address_fiscal_id");

        entity.Property(e => e.CameFromRegisterPostPaid)
            .HasColumnName("came_from_register_post_paid");

        entity.Property(e => e.Cnpj)
            .HasMaxLength(14)
            .HasColumnName("cnpj");

        entity.Property(e => e.CompanyUid)
            .HasMaxLength(36)
            .HasColumnName("company_uid");

        entity.Property(e => e.ExternalId)
            .HasColumnName("external_id");

        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .HasColumnName("name");

        entity.Property(e => e.OnboardDate)
            .HasColumnType("timestamp")
            .HasColumnName("onboard_date");

        entity.Property(e => e.OnboardStatus)
            .HasColumnName("onboard_status");

        entity.Property(e => e.PathSocialContract)
            .HasMaxLength(255)
            .HasColumnName("path_social_contract");

        entity.Property(e => e.PaymentMode)
            .HasColumnName("payment_mode");

        entity.Property(e => e.TransactionReusePeriodMonths)
            .HasColumnName("transaction_reuse_period_months");

        entity.HasIndex(e => e.CompanyUid, "ix_companies_company_uid")
            .IsUnique();
    }
}
