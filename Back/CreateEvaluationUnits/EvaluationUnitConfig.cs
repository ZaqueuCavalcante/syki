namespace Syki.Back.CreateEvaluationUnits;

public class EvaluationUnitConfig : IEntityTypeConfiguration<EvaluationUnit>
{
    public void Configure(EntityTypeBuilder<EvaluationUnit> unit)
    {
        unit.ToTable("evaluation_units");

        unit.HasKey(a => a.Id);
        unit.Property(a => a.Id).ValueGeneratedNever();

        unit.HasMany(a => a.Evaluations)
            .WithOne()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(a => a.UnitId);
    }
}
