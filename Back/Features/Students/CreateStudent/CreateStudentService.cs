using Estud.Back.Domain.Identity;
using Estud.Back.Domain.Students;
using Estud.Back.Domain.Webhooks;

namespace Estud.Back.Features.Students.CreateStudent;

public class CreateStudentService(EstudDbContext ctx, UserManager<EstudUser> userManager) : IEstudService
{
    public async Task<OneOf<CreateStudentOut, EstudError>> Create(CreateStudentIn data)
    {
        var email = data.Email.ToLowerInvariant();
        var emailUsed = await ctx.Users.AnyAsync(u => u.Email == email);
        if (emailUsed) return EmailAlreadyUsed.I;

        var studentRole = await ctx.GetStudentRole();
        var institutionId = ctx.RequestUser.InstitutionId;

        var user = new EstudUser(institutionId, data.Name, email);
        var student = new EstudStudent(user, institutionId, data.Name);
        var userRole = new EstudUserRole(institutionId, user, studentRole.Id);
        ctx.AddRange(student, userRole);

        // TODO: Refactor to use Domain Events Pattern
        var webhookSubscriptions = await ctx.WebhookSubscriptions.Where(x => x.InstitutionId == institutionId && x.IsActive)
            .Select(x => new { x.Id, x.Events }).ToListAsync() ?? [];
        foreach (var webhookSubscription in webhookSubscriptions.Where(x => x.Events.Contains(WebhookEventType.StudentCreated)))
        {
            var webhookCall = new WebhookCall(institutionId, webhookSubscription.Id, new { student.Name, user.Email }, WebhookEventType.StudentCreated);
            ctx.Add(webhookCall);
        }

        await userManager.CreateAsync(user, $"Estud@{Guid.NewGuid()}");

        return new CreateStudentOut { Id = student.Id };
    }
}
