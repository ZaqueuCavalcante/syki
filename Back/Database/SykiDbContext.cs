using Syki.Shared;
using Syki.Back.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Syki.Back.Database;

public class SykiDbContext : IdentityDbContext<SykiUser, SykiRole, Guid>
{
    public DbSet<Faculdade> Faculdades { get; set; }
    public DbSet<Campus> Campi { get; set; }
    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<GradeDisciplina> GradesDisciplinas { get; set; }
    public DbSet<Disciplina> Disciplinas { get; set; }

    public DbSet<Oferta> Ofertas { get; set; }
    public DbSet<Turma> Turmas { get; set; }

    public DbSet<Professor> Professores { get; set; }
    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<Periodo> Periodos { get; set; }

    public DbSet<CursoDisciplina> CursosDisciplinas { get; set; }

    public SykiDbContext(DbContextOptions<SykiDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("syki");

        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        foreach (var entity in builder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName()!.ToSnakeCase().Replace("asp_net_", ""));
        }
    }

    // SEED DATA
    public void SeedStartupData()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();

        DbSeed.NovaRoma.Cursos[1].Disciplinas = DbSeed.NovaRoma.Disciplinas.Take(31).ToList();

        var disciplinasDireito = DbSeed.NovaRoma.Disciplinas.Skip(31).Take(39).ToList();
        disciplinasDireito.Add(DbSeed.NovaRoma.Disciplinas.First(x => x.Nome == "Informática e Sociedade"));
        DbSeed.NovaRoma.Cursos[4].Disciplinas = disciplinasDireito;

        Faculdades.Add(DbSeed.NovaRoma);

        AddRange(DbSeed.Periodos);

        SaveChanges();
    }

    public List<SykiUser> SykiUsers = new()
    {
        new SykiUser { Id = Guid.NewGuid(), Email = "adm@syki.com", },
        new SykiUser { Id = Guid.NewGuid(), Email = "aluno@novaroma.com", FaculdadeId = Guid.Parse("8d08e437-8b18-4a15-a231-4a2260e60432"), },
        new SykiUser { Id = Guid.NewGuid(), Email = "professor@novaroma.com", FaculdadeId = Guid.Parse("8d08e437-8b18-4a15-a231-4a2260e60432"), },
        new SykiUser { Id = Guid.NewGuid(), Email = "academico@novaroma.com", FaculdadeId = Guid.Parse("8d08e437-8b18-4a15-a231-4a2260e60432"), },
    };
}
