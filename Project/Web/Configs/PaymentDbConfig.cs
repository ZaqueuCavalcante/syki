using Exato.Web.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Web.Configs;

public class PaymentDbConfig : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> entity)
    {
        entity.ToTable("payments", "public");

        entity.HasKey(e => e.Id)
            .HasName("pk_payments");

        entity.Property(e => e.Id)
            .UseIdentityColumn()
            .HasColumnName("id");

        entity.Property(e => e.Bonus)
            .HasPrecision(18, 2)
            .HasColumnName("bonus");

        entity.Property(e => e.Comments)
            .HasColumnName("comments");

        entity.Property(e => e.CompanyId)
            .HasColumnName("company_id");

        entity.Property(e => e.CreditPackageId)
            .HasColumnName("credit_package_id");

        entity.Property(e => e.InitialEmailSent)
            .HasDefaultValue(false)
            .HasColumnName("initial_email_sent");

        entity.Property(e => e.LastCallEmailSent)
            .HasColumnName("last_call_email_sent");

        entity.Property(e => e.LastEventDate)
            .HasColumnType("timestamp")
            .HasColumnName("last_event_date");

        entity.Property(e => e.LastEventJson)
            .HasColumnType("jsonb")
            .HasColumnName("last_event_json");

        entity.Property(e => e.PaymentProvider)
            .HasColumnName("payment_provider");

        entity.Property(e => e.Reference)
            .HasColumnName("reference");

        entity.Property(e => e.StartDate)
            .HasColumnType("timestamp")
            .HasColumnName("start_date");

        entity.Property(e => e.Status)
            .HasColumnName("status");

        entity.Property(e => e.TransactionCode)
            .HasColumnName("transaction_code");

        entity.Property(e => e.TransactionDate)
            .HasColumnType("timestamp")
            .HasColumnName("transaction_date");

        entity.Property(e => e.TransactionJson)
            .HasColumnType("jsonb")
            .HasColumnName("transaction_json");

        entity.Property(e => e.UserId)
            .HasColumnName("user_id");

        entity.Property(e => e.Value)
            .HasPrecision(18, 2)
            .HasColumnName("value");

        entity.HasOne<WebUser>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("fk_payments_users_user_id");

        entity.HasOne<Company>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_payments_companies_company_id");

        entity.HasIndex(e => e.CompanyId)
            .HasDatabaseName("ix_payments_company_id");

        entity.HasIndex(e => e.CreditPackageId)
            .HasDatabaseName("ix_payments_credit_package_id");

        entity.HasIndex(e => e.UserId)
            .HasDatabaseName("ix_payments_user_id");
    }
}
