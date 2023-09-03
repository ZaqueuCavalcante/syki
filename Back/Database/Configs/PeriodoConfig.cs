using Syki.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Database;

public class PeriodoConfig : IEntityTypeConfiguration<Periodo>
{
    public void Configure(EntityTypeBuilder<Periodo> periodo)
    {
        periodo.ToTable("periodos");

        periodo.HasKey(p => p.Id);
        periodo.Property(p => p.Id).ValueGeneratedOnAdd();
    }
}
