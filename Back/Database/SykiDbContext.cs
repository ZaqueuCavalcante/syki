using Syki.Back.Audit;
using Audit.EntityFramework;
using Syki.Back.Features.Cross.CreateUser;
using Syki.Back.Features.Academic.CreateClass;
using Syki.Back.Features.Academic.CreateCourse;
using Syki.Back.Features.Academic.CreateCampus;
using Syki.Back.Features.Academic.CreateStudent;
using Syki.Back.Features.Academic.CreateTeacher;
using Syki.Back.Features.Cross.CreateInstitution;
using Syki.Back.Features.Cross.SetupFeatureFlags;
using Syki.Back.Features.Academic.CreateDiscipline;
using Syki.Back.Features.Academic.CreateNotification;
using Syki.Back.Features.Cross.SendResetPasswordToken;
using Syki.Back.Features.Academic.CreateCourseOffering;
using Syki.Back.Features.Academic.CreateAcademicPeriod;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Syki.Back.Features.Teacher.CreateLessonAttendance;
using Syki.Back.Features.Cross.CreatePendingUserRegister;
using Syki.Back.Features.Academic.CreateCourseCurriculum;
using Syki.Back.Features.Academic.CreateEnrollmentPeriod;
using Syki.Back.Features.Student.CreateStudentEnrollment;

namespace Syki.Back.Database;

public class SykiDbContext(DbContextOptions<SykiDbContext> options, DatabaseSettings settings, IHttpContextAccessor httpContextAccessor) : IdentityDbContext<SykiUser, SykiRole, Guid>(options)
{
    public DbSet<Campus> Campi { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<SykiTask> Tasks { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<SykiTeacher> Teachers { get; set; }
    public DbSet<SykiStudent> Students { get; set; }
    public DbSet<ExamGrade> ExamGrades { get; set; }
    public DbSet<Discipline> Disciplines { get; set; }
    public DbSet<DomainEvent> DomainEvents { get; set; }
    public DbSet<Institution> Institutions { get; set; }
    public DbSet<InstitutionConfigs> Configs { get; set; }
    public DbSet<FeatureFlags> FeatureFlags { get; set; }
    public DbSet<UserRegister> UserRegisters { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<LessonAttendance> Attendances { get; set; }
    public DbSet<ClassStudent> ClassesStudents { get; set; }
    public DbSet<AcademicPeriod> AcademicPeriods { get; set; }
    public DbSet<CourseOffering> CourseOfferings { get; set; }
    public DbSet<CourseCurriculum> CourseCurriculums { get; set; }
    public DbSet<EnrollmentPeriod> EnrollmentPeriods { get; set; }
    public DbSet<UserNotification> UserNotifications { get; set; }
    public DbSet<CourseDiscipline> CoursesDisciplines { get; set; }
    public DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; }
    public DbSet<CourseCurriculumDiscipline> CourseCurriculumDisciplines { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(settings.ConnectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.AddInterceptors(new AuditSaveChangesInterceptor());
        optionsBuilder.AddInterceptors(new SaveDomainEventsInterceptor(httpContextAccessor));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("syki");

        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        foreach (var entity in builder.Model.GetEntityTypes())
        {
            if (entity.GetTableName().IsEmpty()) continue;

            entity.SetTableName(entity.GetTableName().ToSnakeCase().Replace("asp_net_", ""));

            foreach (var fk in entity.GetForeignKeys())
            {
                fk.SetConstraintName(fk.GetConstraintName().Replace("1", ""));
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(index.GetDatabaseName()?.ToSnakeCase());
            }
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Enum>().HaveConversion<string>();
    }
}
