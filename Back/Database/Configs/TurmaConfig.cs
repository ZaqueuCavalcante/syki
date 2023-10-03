using Syki.Back.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Back.Database;

public class TurmaConfig : IEntityTypeConfiguration<Turma>
{
    public void Configure(EntityTypeBuilder<Turma> turma)
    {
        turma.ToTable("turmas");

        turma.HasKey(t => t.Id);
        turma.Property(t => t.Id).ValueGeneratedNever();

        turma.HasOne(t => t.Professor)
            .WithMany()
            .HasForeignKey(t => t.ProfessorId);

        turma.HasOne(t => t.Disciplina)
            .WithMany()
            .HasForeignKey(t => t.DisciplinaId);

        turma.HasMany(t => t.Alunos)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                joinEntityName: "turmas__alunos",
                configureLeft: x => x.HasOne<Turma>().WithMany().HasForeignKey("TurmaId"),
                configureRight: x => x.HasOne<Aluno>().WithMany().HasForeignKey("AlunoId")
            );

        turma.HasMany(t => t.Aulas)
            .WithOne()
            .HasForeignKey(a => a.TurmaId);

        turma.HasOne<Periodo>()
            .WithMany()
            .HasForeignKey(t => new { t.Periodo, t.FaculdadeId });
    }
}
