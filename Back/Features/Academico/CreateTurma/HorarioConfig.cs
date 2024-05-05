namespace Syki.Back.CreateTurma;

public class HorarioConfig : IEntityTypeConfiguration<Horario>
{
    public void Configure(EntityTypeBuilder<Horario> horario)
    {
        horario.ToTable("horarios");

        horario.HasKey(h => h.Id);
        horario.Property(h => h.Id).ValueGeneratedNever();
    }
}
