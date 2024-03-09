namespace Syki.Back.CreateCurso;

public class CursoConfig : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> curso)
    {
        curso.ToTable("cursos");

        curso.HasKey(c => c.Id);
        curso.Property(c => c.Id).ValueGeneratedNever();

        curso.HasMany(c => c.Grades)
            .WithOne(g => g.Curso)
            .HasForeignKey(g => g.CursoId);

        curso.HasMany(c => c.Disciplinas)
            .WithMany()
            .UsingEntity<CursoDisciplina>(cd =>
                {
                    cd.ToTable("cursos__disciplinas");
                    cd.HasOne<Curso>().WithMany(c => c.Vinculos).HasForeignKey(x => x.CursoId);
                    cd.HasOne<Disciplina>().WithMany(d => d.Vinculos).HasForeignKey(x => x.DisciplinaId);
                }
            );
    }
}
