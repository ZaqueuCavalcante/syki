using Syki.Dtos;
using Syki.Back.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Syki.Back.Database;

public class OfertaConfig : IEntityTypeConfiguration<Oferta>
{
    public void Configure(EntityTypeBuilder<Oferta> oferta)
    {
        oferta.ToTable("ofertas");

        oferta.HasKey(co => co.Id);
        oferta.Property(co => co.Id).ValueGeneratedNever();

        oferta.HasOne(o => o.Curso)
            .WithMany()
            .HasForeignKey(co => co.CursoId);

        oferta.HasOne(o => o.Grade)
            .WithMany()
            .HasForeignKey(co => co.GradeId);

        oferta.HasOne<Periodo>()
            .WithMany()
            .HasForeignKey(co => new { co.Periodo, co.FaculdadeId });

        oferta.Property(c => c.Turno)
            .HasConversion(new EnumToStringConverter<Turno>());
    }
}
