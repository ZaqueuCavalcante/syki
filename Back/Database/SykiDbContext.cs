using Syki.Domain;
using Syki.Configs;
using Microsoft.EntityFrameworkCore;

namespace Syki.Database;

public class SykiDbContext : DbContext
{
    public DbSet<Faculdade> Faculdades { get; set; }
    public DbSet<Campus> Campi { get; set; }
    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Disciplina> Disciplinas { get; set; }

    public DbSet<Oferta> Ofertas { get; set; }
    public DbSet<Turma> Turmas { get; set; }

    public DbSet<Professor> Professores { get; set; }
    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<Periodo> Periodos { get; set; }

    public DbSet<CursoDisciplina> CursosDisciplinas { get; set; }

    public void SeedStartupData()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
        Faculdades.Add(DbSeed.NovaRoma);
        SaveChanges();
        var novaRoma = Faculdades.First(x => x.Id == 1);

        var disciplinasDireito = DbSeed.NovaRoma.Disciplinas.Skip(31).Take(39).ToList();
        disciplinasDireito.Add(DbSeed.NovaRoma.Disciplinas.First(x => x.Nome == "Informática e Sociedade"));

        var ads = Cursos.First(c => c.Id == 2);
        ads.Disciplinas = DbSeed.NovaRoma.Disciplinas.Take(31).ToList();

        var direito = Cursos.First(c => c.Id == 5);
        direito.Disciplinas = disciplinasDireito;

        AddRange(DbSeed.Periodos);
        AddRange(DbSeed.Professores);
        AddRange(DbSeed.Alunos);

        SaveChanges();
    }

    public List<User> Users = new()
    {
        new User { Id = 0, Email = "adm@syki.com", Role = AuthorizationConfigs.Adm },
        new User { Id = 1, Email = "aluno@novaroma.com", FaculdadeId = 1, Role = AuthorizationConfigs.Aluno },
        new User { Id = 2, Email = "professor@novaroma.com", FaculdadeId = 1, Role = AuthorizationConfigs.Professor },
        new User { Id = 3, Email = "academico@novaroma.com", FaculdadeId = 1, Role = AuthorizationConfigs.Academico },
        new User { Id = 4, Email = "aluno@ufpe.com", FaculdadeId = 2, Role = AuthorizationConfigs.Aluno },
        new User { Id = 5, Email = "professor@ufpe.com", FaculdadeId = 2, Role = AuthorizationConfigs.Professor },
        new User { Id = 6, Email = "academico@ufpe.com", FaculdadeId = 2, Role = AuthorizationConfigs.Academico },
    };

    public SykiDbContext(DbContextOptions<SykiDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("syki");

        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
