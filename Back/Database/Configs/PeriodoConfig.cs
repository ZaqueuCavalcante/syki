using Syki.Back.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Back.Database;

public class PeriodoConfig : IEntityTypeConfiguration<Periodo>
{
    public void Configure(EntityTypeBuilder<Periodo> periodo)
    {
        periodo.ToTable("periodos");

        periodo.HasKey(p => new { p.Id, p.FaculdadeId });
        periodo.Property(p => p.Id).ValueGeneratedNever();

        periodo.HasOne<PeriodoDeMatricula>()
            .WithOne()
            .HasPrincipalKey<Periodo>(p => new { p.Id, p.FaculdadeId })
            .HasForeignKey<PeriodoDeMatricula>(pm => new { pm.Id, pm.FaculdadeId });
    }
}
