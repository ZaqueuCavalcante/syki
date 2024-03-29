namespace Syki.Back.Database;

public class AulaConfig : IEntityTypeConfiguration<Aula>
{
    public void Configure(EntityTypeBuilder<Aula> aula)
    {
        aula.ToTable("aulas");

        aula.HasKey(a => a.Id);
        aula.Property(a => a.Id).ValueGeneratedNever();
    }
}
