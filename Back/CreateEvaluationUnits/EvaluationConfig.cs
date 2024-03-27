namespace Syki.Back.CreateEvaluationUnits;

public class EvaluationConfig : IEntityTypeConfiguration<Evaluation>
{
    public void Configure(EntityTypeBuilder<Evaluation> evaluation)
    {
        evaluation.ToTable("evaluations");

        evaluation.HasKey(a => a.Id);
        evaluation.Property(a => a.Id).ValueGeneratedNever();
    }
}
