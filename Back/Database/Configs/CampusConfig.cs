using Syki.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Database;

public class CampusConfig : IEntityTypeConfiguration<Campus>
{
    public void Configure(EntityTypeBuilder<Campus> campus)
    {
        campus.ToTable("campi");

        campus.HasKey(c => c.Id);
        campus.Property(c => c.Id).ValueGeneratedOnAdd();

        campus.HasMany<Oferta>()
            .WithOne(o => o.Campus)
            .HasForeignKey(a => a.CampusId);
    }
}
