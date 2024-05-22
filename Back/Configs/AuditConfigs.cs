using Audit.Core;
using Syki.Back.Audit;
using Syki.Back.Features.Cross.CreateUser;
using Syki.Back.Features.Academic.CreateClass;
using Syki.Back.Features.Academic.CreateCourse;
using Syki.Back.Features.Academic.CreateCampus;
using Syki.Back.Features.Academic.CreateTeacher;
using Syki.Back.Features.Academic.CreateStudent;
using Syki.Back.Features.Cross.CreateInstitution;
using Syki.Back.Features.Academic.CreateDiscipline;
using Syki.Back.Features.Academic.CreateNotification;
using Syki.Back.Features.Academic.CreateCourseOffering;
using Syki.Back.Features.Academic.CreateCourseCurriculum;

namespace Syki.Back.Configs;

public static class AuditConfigs
{
    public static void AddAuditConfigs(this IServiceCollection services)
    {
        var context = services.BuildServiceProvider().GetService<SykiDbContext>();

        Configuration.Setup().UseEntityFramework(_ => _
            .UseDbContext(_ => context)
            .AuditTypeExplicitMapper(_ => _
                .Map<Class, AuditLog>()
                .Map<Campus, AuditLog>()
                .Map<Course, AuditLog>()
                .Map<Schedule, AuditLog>()
                .Map<SykiUser, AuditLog>()
                .Map<Discipline, AuditLog>()
                .Map<SykiStudent, AuditLog>()
                .Map<Institution, AuditLog>()
                .Map<SykiTeacher, AuditLog>()
                .Map<Notification, AuditLog>()
                .Map<CourseOffering, AuditLog>()
                .Map<CourseCurriculum, AuditLog>()
                .Map<CourseDiscipline, AuditLog>()
                .Map<CourseCurriculumDiscipline, AuditLog>()
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
