using Syki.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Database;

public class PeriodoConfig : IEntityTypeConfiguration<Periodo>
{
    public void Configure(EntityTypeBuilder<Periodo> periodo)
    {
        periodo.ToTable("periodos");

        periodo.HasKey(p => new { p.Id, p.FaculdadeId });
        periodo.Property(p => p.Id).ValueGeneratedNever();

        periodo.HasOne<Faculdade>()
            .WithMany()
            .HasForeignKey(p => p.FaculdadeId);
    }
}
