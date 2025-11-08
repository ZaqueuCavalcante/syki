using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class OrganizationSegmentDbConfig : IEntityTypeConfiguration<OrganizationSegment>
{
    public void Configure(EntityTypeBuilder<OrganizationSegment> entity)
    {
        entity.ToTable("organization_segment", "public");

        entity.HasKey(e => e.Id)
            .HasName("organization_segment_pkey");

        entity.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        entity.Property(e => e.Name)
            .HasColumnType("varchar")
            .HasColumnName("name");
    }
}
