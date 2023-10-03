using Syki.Back.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Back.Database;

public class OfertaTurmaConfig : IEntityTypeConfiguration<OfertaTurma>
{
    public void Configure(EntityTypeBuilder<OfertaTurma> ofertaTurma)
    {
        ofertaTurma.ToTable("ofertas__turmas");

        ofertaTurma.HasKey(ot => new { ot.OfertaId, ot.TurmaId });

        ofertaTurma.HasOne<Oferta>()
            .WithMany()
            .HasForeignKey(ot => ot.OfertaId);

        ofertaTurma.HasOne<Turma>()
            .WithMany()
            .HasForeignKey(ot => ot.TurmaId);
    }
}
