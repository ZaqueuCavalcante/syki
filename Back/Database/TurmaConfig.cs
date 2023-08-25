using Syki.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Database;

public class TurmaConfig : IEntityTypeConfiguration<Turma>
{
    public void Configure(EntityTypeBuilder<Turma> turma)
    {
        turma.ToTable("turmas");

        turma.HasKey(t => t.Id);
        turma.Property(t => t.Id).ValueGeneratedOnAdd();

        turma.HasOne<Professor>()
            .WithMany()
            .HasForeignKey(t => t.ProfessorId);

        turma.HasOne<Disciplina>()
            .WithMany()
            .HasForeignKey(t => t.DisciplinaId);

        turma.HasOne<Curso>()
            .WithMany()
            .HasForeignKey(t => t.CursoId);

        turma.HasMany(t => t.Alunos)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                joinEntityName: "alunos_turmas",
                configureLeft: x => x.HasOne<Turma>().WithMany().HasForeignKey("TurmaId"),
                configureRight: x => x.HasOne<Aluno>().WithMany().HasForeignKey("AlunoId")
            );
    }
}
