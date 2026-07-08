using Syki.Back.Domain.Identity;
using Syki.Back.Domain.Students;
using Syki.Back.Domain.Webhooks;

namespace Syki.Back.Features.Students.CreateStudent;

public class CreateStudentService(SykiDbContext ctx, UserManager<SykiUser> userManager) : ISykiService
{
    public async Task<OneOf<CreateStudentOut, SykiError>> Create(CreateStudentIn data)
    {
        var email = data.Email.ToLowerInvariant();
        var emailUsed = await ctx.Users.AnyAsync(u => u.Email == email);
        if (emailUsed) return EmailAlreadyUsed.I;

        var studentRole = await ctx.GetStudentRole();
        var institutionId = ctx.RequestUser.InstitutionId;

        var user = new SykiUser(institutionId, data.Name, email);
        var student = new SykiStudent(user, institutionId, data.Name);
        var userRole = new SykiUserRole(institutionId, user, studentRole.Id);
        ctx.AddRange(student, userRole);

        // TODO: Refactor to use Domain Events Pattern
        var webhookSubscriptions = await ctx.WebhookSubscriptions.Where(x => x.InstitutionId == institutionId && x.IsActive)
            .Select(x => new { x.Id, x.Events }).ToListAsync() ?? [];
        foreach (var webhookSubscription in webhookSubscriptions.Where(x => x.Events.Contains(WebhookEventType.StudentCreated)))
        {
            var webhookCall = new WebhookCall(institutionId, webhookSubscription.Id, new { student.Name, user.Email }, WebhookEventType.StudentCreated);
            ctx.Add(webhookCall);
        }

        await userManager.CreateAsync(user, $"Syki@{Guid.NewGuid()}");

        return new CreateStudentOut { Id = student.Id };
    }
}
