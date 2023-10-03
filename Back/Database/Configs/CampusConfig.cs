using Syki.Back.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Back.Database;

public class CampusConfig : IEntityTypeConfiguration<Campus>
{
    public void Configure(EntityTypeBuilder<Campus> campus)
    {
        campus.ToTable("campi");

        campus.HasKey(c => c.Id);
        campus.Property(c => c.Id).ValueGeneratedNever();

        campus.HasMany<Oferta>()
            .WithOne(o => o.Campus)
            .HasForeignKey(a => a.CampusId);
    }
}
