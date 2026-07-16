using Estud.Back.Domain.Parents;
using Estud.Back.Domain.Identity;

namespace Estud.Back.Features.Parents.CreateParent;

public class CreateParentService(EstudDbContext ctx, UserManager<EstudUser> userManager) : IEstudService
{
    private class Validator : AbstractValidator<CreateParentIn>
    {
        public Validator()
        {
            RuleFor(x => x.Email).Must(x => x.IsValidEmail()).WithError(InvalidEmail.I);

            When(x => x.PhoneNumber.HasValue(), () =>
            {
                RuleFor(x => x.PhoneNumber).Must(x => x.IsValidPhoneNumber()).WithError(InvalidPhoneNumber.I);
            });

            RuleFor(x => x.Students).NotEmpty().WithError(InvalidParentStudentsList.I);
            RuleFor(x => x.Students).Must(x => x.Select(s => s.StudentId).IsAllDistinct()).WithError(InvalidParentStudentsList.I);
            RuleFor(x => x.Students).Must(x => x.All(s => Enum.IsDefined(s.Relationship))).WithError(InvalidParentRelationship.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CreateParentOut, EstudError>> Create(CreateParentIn data)
    {
        if (V.Run(data, out var error)) return error;

        var email = data.Email.ToLowerInvariant();
        var emailUsed = await ctx.Users.AnyAsync(u => u.Email == email);
        if (emailUsed) return EmailAlreadyUsed.I;

        var institutionId = ctx.RequestUser.InstitutionId;

        var studentIds = data.Students.ConvertAll(s => s.StudentId);
        var existingStudents = await ctx.Students.CountAsync(s => s.InstitutionId == institutionId && studentIds.Contains(s.Id));
        if (existingStudents != studentIds.Count) return StudentNotFound.I;

        var parentRole = await ctx.GetParentRole();

        var user = new EstudUser(institutionId, data.Name, email)
        {
            PhoneNumber = data.PhoneNumber.HasValue() ? data.PhoneNumber : null,
        };
        var parent = new EstudParent(user, institutionId, data.Name);
        var userRole = new EstudUserRole(institutionId, user, parentRole.Id);
        ctx.AddRange(parent, userRole);

        foreach (var student in data.Students)
        {
            ctx.Add(new ParentStudent(institutionId, parent, student.StudentId, student.Relationship));
        }

        await userManager.CreateAsync(user, $"Estud@{Guid.NewGuid()}");

        return new CreateParentOut { Id = parent.Id };
    }
}
