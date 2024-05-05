namespace Syki.Back.CreateAluno;

public class AlunoConfig : IEntityTypeConfiguration<Aluno>
{
    public void Configure(EntityTypeBuilder<Aluno> aluno)
    {
        aluno.ToTable("alunos");

        aluno.HasKey(a => a.Id);
        aluno.Property(a => a.Id).ValueGeneratedNever();

        aluno.Property(a => a.OfertaId);
        aluno.Property(a => a.Name);
        aluno.Property(a => a.Matricula);

        aluno.HasOne(a => a.User)
            .WithOne()
            .HasPrincipalKey<SykiUser>(u => new { u.InstitutionId, u.Id })
            .HasForeignKey<Aluno>(a => new { a.InstitutionId, a.Id });
    }
}
