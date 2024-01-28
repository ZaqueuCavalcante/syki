using Syki.Back.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Back.Database;

public class HorarioConfig : IEntityTypeConfiguration<Horario>
{
    public void Configure(EntityTypeBuilder<Horario> horario)
    {
        horario.ToTable("horarios");

        horario.HasKey(h => h.Id);
        horario.Property(h => h.Id).ValueGeneratedNever();
    }
}
