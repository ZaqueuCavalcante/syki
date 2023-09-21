using Syki.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Database;

public class AlunoConfig : IEntityTypeConfiguration<Aluno>
{
    public void Configure(EntityTypeBuilder<Aluno> aluno)
    {
        aluno.ToTable("alunos");

        aluno.HasKey(a => a.Id);
        aluno.Property(a => a.Id).ValueGeneratedOnAdd();
    }
}
