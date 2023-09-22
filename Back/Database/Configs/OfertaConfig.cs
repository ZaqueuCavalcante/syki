using Syki.Dtos;
using Syki.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Syki.Database;

public class OfertaConfig : IEntityTypeConfiguration<Oferta>
{
    public void Configure(EntityTypeBuilder<Oferta> oferta)
    {
        oferta.ToTable("ofertas");

        oferta.HasKey(co => co.Id);
        oferta.Property(co => co.Id).ValueGeneratedOnAdd();

        oferta.HasOne(o => o.Curso)
            .WithMany()
            .HasForeignKey(co => co.CursoId);

        oferta.HasOne(o => o.Grade)
            .WithMany()
            .HasForeignKey(co => co.GradeId);

        oferta.HasOne<Periodo>()
            .WithMany()
            .HasForeignKey(co => new { co.PeriodoId, co.FaculdadeId });

        oferta.Property(c => c.Turno)
            .HasConversion(new EnumToStringConverter<Turno>());
    }
}
