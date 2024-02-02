using Syki.Back.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Back.Database;

public class AlunoConfig : IEntityTypeConfiguration<Aluno>
{
    public void Configure(EntityTypeBuilder<Aluno> aluno)
    {
        aluno.ToTable("alunos");

        aluno.HasKey(a => a.Id);
        aluno.Property(a => a.Id).ValueGeneratedNever();

        aluno.HasOne(a => a.User)
            .WithOne()
            .HasPrincipalKey<SykiUser>(u => new { u.FaculdadeId, u.Id })
            .HasForeignKey<Aluno>(a => new { a.FaculdadeId, a.UserId });

        aluno.HasMany<Disciplina>()
            .WithMany()
            .UsingEntity<AlunoDisciplina>(ad =>
                {
                    ad.ToTable("alunos__disciplinas");
                    ad.HasOne<Aluno>().WithMany().HasForeignKey(x => x.AlunoId);
                    ad.HasOne<Disciplina>().WithMany().HasForeignKey(x => x.DisciplinaId);
                }
            );
    }
}
