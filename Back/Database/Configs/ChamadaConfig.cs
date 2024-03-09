using Syki.Back.CreateAluno;

namespace Syki.Back.Database;

public class ChamadaConfig : IEntityTypeConfiguration<Chamada>
{
    public void Configure(EntityTypeBuilder<Chamada> chamada)
    {
        chamada.ToTable("chamadas");

        chamada.HasKey(c => new { c.AulaId, c.AlunoId });

        chamada.HasOne<Aula>()
            .WithMany()
            .HasForeignKey(c => c.AulaId);

        chamada.HasOne<Aluno>()
            .WithMany()
            .HasForeignKey(c => c.AlunoId);
    }
}
