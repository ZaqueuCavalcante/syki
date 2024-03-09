namespace Syki.Back.CreateDisciplina;

public class DisciplinaConfig : IEntityTypeConfiguration<Disciplina>
{
    public void Configure(EntityTypeBuilder<Disciplina> disciplina)
    {
        disciplina.ToTable("disciplinas");

        disciplina.HasKey(d => d.Id);
        disciplina.Property(d => d.Id).ValueGeneratedNever();
    }
}
