using Audit.Core;
using Syki.Back.Domain;
using System.Text.Json;
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
                .Map<OfertaTurma, AuditLog>()
                .Map<Periodo, AuditLog>()
                .Map<Professor, AuditLog>()
                .Map<SykiUser, AuditLog>()
                .Map<Turma, AuditLog>()
                .AuditEntityAction<AuditLog>((ev, entry, log) =>
                {
                    log.Id = Guid.NewGuid();
                    log.EntityId = Guid.Parse(entry.PrimaryKey.First().Value.ToString()!);
                    log.EntityType = entry.EntityType.Name;
                    log.CreatedAt = DateTime.Now;
                    log.UserId = Guid.Parse(ev.CustomFields["UserId"].ToString()!);
                    log.FaculdadeId = Guid.Parse(ev.CustomFields["FaculdadeId"].ToString()!);
                    log.Data = JsonDocument.Parse(entry.ToJson());
                }))
        .IgnoreMatchedProperties(true));
    }

    public static void UseAudit(this IApplicationBuilder app)
    {
        var contextAccessor = app.ApplicationServices.GetService<IHttpContextAccessor>()!;

        Configuration.AddCustomAction(ActionType.OnScopeCreated, scope =>
        {
            var httpContext = contextAccessor.HttpContext!;
            scope.Event.CustomFields["UserId"] = httpContext.User.Id();
            scope.Event.CustomFields["FaculdadeId"] = httpContext.User.Facul();
        });
    }
}
