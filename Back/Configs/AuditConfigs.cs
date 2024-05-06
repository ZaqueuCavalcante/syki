using Audit.Core;
using Syki.Back.Audit;
using Syki.Back.CreateTurma;
using Syki.Back.CreateOferta;
using Syki.Back.CreateProfessor;
using Syki.Back.Features.Cross.CreateInstitution;
using Syki.Back.CreateNotification;
using Syki.Back.Features.Academic.CreateGrade;
using Syki.Back.Features.Academic.CreateCurso;
using Syki.Back.Features.Academic.CreateCampus;
using Syki.Back.Features.Academic.CreateStudent;
using Syki.Back.Features.Academic.CreateDisciplina;

namespace Syki.Back.Configs;

public static class AuditConfigs
{
    public static void AddAuditConfigs(this IServiceCollection services)
    {
        var context = services.BuildServiceProvider().GetService<SykiDbContext>();

        Configuration.Setup().UseEntityFramework(_ => _
            .UseDbContext(_ => context)
            .AuditTypeExplicitMapper(_ => _
                .Map<Aluno, AuditLog>()
                .Map<Curso, AuditLog>()
                .Map<Grade, AuditLog>()
                .Map<Turma, AuditLog>()
                .Map<Campus, AuditLog>()
                .Map<Oferta, AuditLog>()
                .Map<Horario, AuditLog>()
                .Map<SykiUser, AuditLog>()
                .Map<Professor, AuditLog>()
                .Map<Disciplina, AuditLog>()
                .Map<Institution, AuditLog>()
                .Map<Notification, AuditLog>()
                .Map<CursoDisciplina, AuditLog>()
                .Map<GradeDisciplina, AuditLog>()
                .AuditEntityAction<AuditLog>((evt, entry, log) => log.Fill(evt, entry)))
            .IgnoreMatchedProperties(true));
    }

    public static void UseAudit(this IApplicationBuilder app)
    {
        var contextAccessor = app.ApplicationServices.GetService<IHttpContextAccessor>()!;

        Configuration.AddCustomAction(ActionType.OnScopeCreated, scope =>
        {
            var httpContext = contextAccessor.HttpContext;
            if (httpContext == null) return;

            if (!httpContext.Request.Path.IsAuditable())
            {
                scope.Event.CustomFields["Skip"] = true;
                return;
            }

            scope.Event.CustomFields["UserId"] = httpContext.User.Id();
            scope.Event.CustomFields["InstitutionId"] = httpContext.User.InstitutionId();
        });
    }
}
