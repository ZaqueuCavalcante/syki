using Audit.Core;
using Syki.Back.Audit;
using Syki.Back.CreateAluno;
using Syki.Back.CreateTurma;
using Syki.Back.CreateOferta;
using Syki.Back.CreateProfessor;
using Syki.Back.CreateInstitution;
using Syki.Back.CreateNotification;
using Syki.Back.Features.Academico.CreateCurso;
using Syki.Back.Features.Academico.CreateCampus;
using Syki.Back.Features.Academico.CreateDisciplina;
using Syki.Back.Features.Academico.CreateGrade;

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
                .Map<Campus, AuditLog>()
                .Map<Curso, AuditLog>()
                .Map<CursoDisciplina, AuditLog>()
                .Map<Disciplina, AuditLog>()
                .Map<Institution, AuditLog>()
                .Map<Grade, AuditLog>()
                .Map<GradeDisciplina, AuditLog>()
                .Map<Oferta, AuditLog>()
                .Map<Professor, AuditLog>()
                .Map<SykiUser, AuditLog>()
                .Map<Turma, AuditLog>()
                .Map<Horario, AuditLog>()
                .Map<Notification, AuditLog>()
                .AuditEntityAction<AuditLog>((evt, entry, log) => {
                    return log.Fill(evt, entry); }))
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
