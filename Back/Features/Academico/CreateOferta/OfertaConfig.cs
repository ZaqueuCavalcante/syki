using Syki.Back.CreateAluno;
using Syki.Back.Features.Academic.CreateAcademicPeriod;

namespace Syki.Back.CreateOferta;

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

        oferta.HasOne<AcademicPeriod>()
            .WithMany()
            .HasForeignKey(o => new { o.Periodo, o.InstitutionId });

        oferta.HasMany<Aluno>()
            .WithOne(a => a.Oferta)
            .HasForeignKey(a => a.OfertaId);
    }
}
