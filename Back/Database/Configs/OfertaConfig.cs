using Syki.Back.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Back.Database;

public class OfertaConfig : IEntityTypeConfiguration<Oferta>
{
    public void Configure(EntityTypeBuilder<Oferta> oferta)
    {
        oferta.ToTable("ofertas");

        oferta.HasKey(o => o.Id);
        oferta.Property(o => o.Id).ValueGeneratedNever();

        oferta.HasOne(o => o.Curso)
            .WithMany()
            .HasForeignKey(o => o.CursoId);

        oferta.HasOne(o => o.Grade)
            .WithMany()
            .HasForeignKey(o => o.GradeId);

        oferta.HasOne<Periodo>()
            .WithMany()
            .HasForeignKey(o => new { o.Periodo, o.FaculdadeId });

        oferta.HasMany<Aluno>()
            .WithOne(a => a.Oferta)
            .HasForeignKey(a => a.OfertaId);
    }
}
