using Syki.Shared;
using Syki.Back.Audit;
using Syki.Back.Domain;
using Syki.Back.Settings;
using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Syki.Back.Database;

public class SykiDbContext : IdentityDbContext<SykiUser, SykiRole, Guid>
{
    private readonly DatabaseSettings _settings;
    public SykiDbContext(DbContextOptions<SykiDbContext> options, DatabaseSettings settings) : base(options)
    {
        _settings = settings;
    }

    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<Faculdade> Faculdades { get; set; }
    public DbSet<Campus> Campi { get; set; }
    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<GradeDisciplina> GradesDisciplinas { get; set; }
    public DbSet<Disciplina> Disciplinas { get; set; }
    public DbSet<Oferta> Ofertas { get; set; }
    public DbSet<Turma> Turmas { get; set; }
    public DbSet<TurmaAluno> TurmaAlunos { get; set; }
    public DbSet<Livro> Livros { get; set; }
    public DbSet<Professor> Professores { get; set; }
    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<Periodo> Periodos { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<UserNotification> UserNotifications { get; set; }
    public DbSet<CursoDisciplina> CursosDisciplinas { get; set; }
    public DbSet<ResetPassword> ResetPasswords { get; set; }
    public DbSet<PeriodoDeMatricula> PeriodosDeMatricula { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_settings.ConnectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.AddInterceptors(new AuditSaveChangesInterceptor());
    }

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

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Enum>().HaveConversion<string>();
    }
}
