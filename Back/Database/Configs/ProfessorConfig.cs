using Syki.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Database;

public class ProfessorConfig : IEntityTypeConfiguration<Professor>
{
    public void Configure(EntityTypeBuilder<Professor> professor)
    {
        professor.ToTable("professores");

        professor.HasKey(p => p.Id);
        professor.Property(p => p.Id).ValueGeneratedNever();
    }
}
