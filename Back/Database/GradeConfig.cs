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
            .UsingEntity<Dictionary<string, object>>(
                joinEntityName: "grades__disciplinas",
                configureLeft: x => x.HasOne<Grade>().WithMany().HasForeignKey("GradeId"),
                configureRight: x => x.HasOne<Disciplina>().WithMany().HasForeignKey("DisciplinaId")
            );
    }
}
