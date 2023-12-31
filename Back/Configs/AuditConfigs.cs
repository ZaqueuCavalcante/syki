using Audit.Core;
using Syki.Back.Audit;
using Syki.Back.Domain;
using Syki.Back.Database;
using Syki.Back.Extensions;

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
                .Map<AlunoDisciplina, AuditLog>()
                .Map<Aula, AuditLog>()
                .Map<Campus, AuditLog>()
                .Map<Chamada, AuditLog>()
                .Map<Curso, AuditLog>()
                .Map<CursoDisciplina, AuditLog>()
                .Map<Disciplina, AuditLog>()
                .Map<Faculdade, AuditLog>()
                .Map<Grade, AuditLog>()
                .Map<GradeDisciplina, AuditLog>()
                .Map<Livro, AuditLog>()
                .Map<Oferta, AuditLog>()
                .Map<Periodo, AuditLog>()
                .Map<Professor, AuditLog>()
                .Map<SykiUser, AuditLog>()
                .Map<Turma, AuditLog>()
                .Map<Notification, AuditLog>()
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

            if (httpContext.Request.Path.IsLogin())
            {
                scope.Event.CustomFields["IsLogin"] = true;
                return;
            }

            scope.Event.CustomFields["UserId"] = httpContext.User.Id();
            scope.Event.CustomFields["FaculdadeId"] = httpContext.User.Facul();
        });
    }
}
