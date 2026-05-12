namespace Syki.Back.Features.Academic.CreateDiscipline;

public class DisciplineConfig : IEntityTypeConfiguration<Discipline>
{
    public void Configure(EntityTypeBuilder<Discipline> discipline)
    {
        discipline.ToTable("disciplines");

        discipline.HasKey(d => d.Id);
        discipline.Property(d => d.Id).ValueGeneratedNever();
    }
}
