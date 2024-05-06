using Syki.Back.Audit;
using Audit.EntityFramework;
using Syki.Back.Features.Academic.CreateStudent;
using Syki.Back.CreateTurma;
using Syki.Back.CreateOferta;
using Syki.Back.CreateProfessor;
using Syki.Back.CreateInstitution;
using Syki.Back.CreateNotification;
using Syki.Back.Features.Academic.CreateAcademicPeriod;
using Syki.Back.CreateMatriculaAluno;
using Syki.Back.CreateEnrollmentPeriod;
using Syki.Back.Features.Academico.CreateGrade;
using Syki.Back.Features.Academico.CreateCurso;
using Syki.Back.Features.Academico.CreateCampus;
using Syki.Back.Features.Academico.CreateDisciplina;
using Syki.Back.Features.Cross.SendResetPasswordToken;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Syki.Back.Features.Cross.CreatePendingUserRegister;

namespace Syki.Back.Database;

public class SykiDbContext(DbContextOptions<SykiDbContext> options, DatabaseSettings settings) : IdentityDbContext<SykiUser, SykiRole, Guid>(options)
{
    public DbSet<Campus> Campi { get; set; }
    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<Turma> Turmas { get; set; }
    public DbSet<Oferta> Ofertas { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<Professor> Professores { get; set; }
    public DbSet<TurmaAluno> TurmaAlunos { get; set; }
    public DbSet<Disciplina> Disciplinas { get; set; }
    public DbSet<Institution> Institutions { get; set; }
    public DbSet<UserRegister> UserRegisters { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<AcademicPeriod> AcademicPeriods { get; set; }
    public DbSet<CursoDisciplina> CursosDisciplinas { get; set; }
    public DbSet<GradeDisciplina> GradesDisciplinas { get; set; }
    public DbSet<EnrollmentPeriod> EnrollmentPeriods { get; set; }
    public DbSet<UserNotification> UserNotifications { get; set; }
    public DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(settings.ConnectionString);
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

    public async Task ResetDbAsync()
    {
        if (Env.IsTesting())
        {
            await Database.EnsureDeletedAsync();
            await Database.EnsureCreatedAsync();
        }
    }

    public void ResetDb()
    {
        if (Env.IsDevelopment())
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }

    public void MigrateDb()
    {
        if (!Env.IsTesting())
        {
            Database.Migrate();
        }
    }
}
