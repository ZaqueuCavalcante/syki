using Syki.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Syki.Database;

public class GradeConfig : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> grade)
    {
        grade.ToTable("grades");

        grade.HasKey(g => g.Id);
        grade.Property(g => g.Id).ValueGeneratedOnAdd();

        grade.HasMany(g => g.Disciplinas)
            .WithMany()
            .UsingEntity<GradeDisciplina>(gd =>
                {
                    gd.ToTable("grades__disciplinas");
                    gd.HasOne<Grade>().WithMany().HasForeignKey(x => x.GradeId);
                    gd.HasOne<Disciplina>().WithMany().HasForeignKey(x => x.DisciplinaId);
                }
            );
    }
}
