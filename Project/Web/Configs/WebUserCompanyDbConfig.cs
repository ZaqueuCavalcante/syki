using Exato.Web.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Web.Configs;

public class WebUserCompanyDbConfig : IEntityTypeConfiguration<WebUserCompany>
{
    public void Configure(EntityTypeBuilder<WebUserCompany> entity)
    {
        entity.ToTable("user_companies", "public");

        entity.HasKey(e => e.Id)
            .HasName("pk_user_companies");

        entity.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        entity.Property(e => e.CompanyId)
            .HasColumnName("company_id");

        entity.Property(e => e.IndicationCode)
            .HasMaxLength(12)
            .HasColumnName("indication_code");

        entity.Property(e => e.Main)
            .HasColumnName("main");

        entity.Property(e => e.OrganizationExternalId)
            .HasMaxLength(60)
            .HasColumnName("organization_external_id");

        entity.Property(e => e.PaymentProviderId)
            .HasMaxLength(255)
            .HasColumnName("payment_provider_id");

        entity.Property(e => e.Token)
            .HasMaxLength(32)
            .HasColumnName("token");

        entity.Property(e => e.UserExternalId)
            .HasMaxLength(60)
            .HasColumnName("user_external_id");

        entity.Property(e => e.UserId)
            .HasColumnName("user_id");

        entity.Property(e => e.UserRole)
            .HasColumnName("user_role");

        entity.HasOne<WebUser>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(d => d.UserId);

        entity.HasOne<Company>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(d => d.CompanyId);

        entity.HasIndex(e => new { e.CompanyId, e.UserId })
            .HasDatabaseName("ix_user_companies_company_id_user_id")
            .IsUnique();
    }
}
