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
    }
}
