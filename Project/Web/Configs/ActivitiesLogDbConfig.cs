using Exato.Web.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Web.Configs;

public class ActivitiesLogDbConfig : IEntityTypeConfiguration<ActivitiesLog>
{
    public void Configure(EntityTypeBuilder<ActivitiesLog> entity)
    {
        entity.ToTable("activities_logs", "public");

        entity.HasKey(e => e.Id)
            .HasName("pk_activities_logs");

        entity.Property(e => e.Id)
            .UseIdentityColumn()
            .HasColumnName("id");

        entity.Property(e => e.CompanyId)
            .HasColumnName("company_id");

        entity.Property(e => e.Description)
            .HasMaxLength(300)
            .HasColumnName("description");

        entity.Property(e => e.EventDate)
            .HasColumnType("timestamp")
            .HasColumnName("event_date");

        entity.Property(e => e.IpAddress)
            .HasMaxLength(45)
            .HasColumnName("ip_address");

        entity.Property(e => e.SystemDomainId)
            .HasColumnName("system_domain_id");

        entity.Property(e => e.UserId)
            .HasColumnName("user_id");

        entity.HasIndex(e => e.CompanyId)
            .HasDatabaseName("ix_activities_logs_company_id");

        entity.HasIndex(e => e.SystemDomainId)
            .HasDatabaseName("ix_activities_logs_system_domain_id");

        entity.HasIndex(e => e.UserId)
            .HasDatabaseName("ix_activities_logs_user_id");
    }
}
