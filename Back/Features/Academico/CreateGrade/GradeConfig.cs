using Syki.Back.Features.Academico.CreateDisciplina;

namespace Syki.Back.Features.Academico.CreateGrade;

public class GradeConfig : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> grade)
    {
        grade.ToTable("grades");

        grade.HasKey(g => g.Id);
        grade.Property(g => g.Id).ValueGeneratedNever();

        grade.HasMany(g => g.Disciplinas)
            .WithMany()
            .UsingEntity<GradeDisciplina>(gd =>
                {
                    gd.ToTable("grades__disciplinas");
                    gd.HasOne<Grade>().WithMany(g => g.Vinculos).HasForeignKey(x => x.GradeId);
                    gd.HasOne<Disciplina>().WithMany().HasForeignKey(x => x.DisciplinaId);
                }
            );
    }
}
