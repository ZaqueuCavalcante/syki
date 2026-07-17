namespace Estud.Back.Features.Teachers.UpdateTeacher;

public class UpdateTeacherService(EstudDbContext ctx) : IEstudService
{
    private class Validator : AbstractValidator<UpdateTeacherIn>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithError(InvalidTeacherName.I);
            RuleFor(x => x.Name).MaximumLength(100).WithError(InvalidTeacherName.I);
            RuleFor(x => x.Email).Must(x => x.IsValidEmail()).WithError(InvalidEmail.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<EstudSuccess, EstudError>> Update(int id, UpdateTeacherIn data)
    {
        if (V.Run(data, out var error)) return error;

        var teacher = await ctx.Teachers.Include(t => t.User)
            .FirstOrDefaultAsync(t => t.InstitutionId == ctx.RequestUser.InstitutionId && t.Id == id);
        if (teacher == null) return TeacherNotFound.I;

        var email = data.Email.ToLowerInvariant();
        var emailUsed = await ctx.Users.AnyAsync(u => u.Email == email && u.Id != teacher.UserId);
        if (emailUsed) return EmailAlreadyUsed.I;

        teacher.Name = data.Name;
        teacher.User.Name = data.Name;
        teacher.User.Email = email;
        teacher.User.NormalizedEmail = email.ToUpperInvariant();
        teacher.User.UserName = email;
        teacher.User.NormalizedUserName = email.ToUpperInvariant();

        await ctx.SaveChangesAsync();

        return EstudSuccess.I;
    }
}
