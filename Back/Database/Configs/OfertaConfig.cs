using Syki.Back.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

        oferta.HasMany<Aluno>()
            .WithOne(a => a.Oferta)
            .HasForeignKey(a => a.OfertaId);
    }
}
