namespace Estud.Back.Features.Students.CreateClassActivityWork;

public class CreateClassActivityWorkService(EstudDbContext ctx) : IEstudService
{
    private class Validator : AbstractValidator<CreateClassActivityWorkIn>
    {
        public Validator()
        {
            RuleFor(x => x.Link).NotEmpty().WithError(InvalidClassActivityWorkLink.I);
            RuleFor(x => x.Link).MaximumLength(500).WithError(InvalidClassActivityWorkLink.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CreateClassActivityWorkOut, EstudError>> Create(int id, CreateClassActivityWorkIn data)
    {
        if (V.Run(data, out var error)) return error;

        var userId = ctx.RequestUser.Id;
        var institutionId = ctx.RequestUser.InstitutionId;
        var studentId = await ctx.GetStudentId(institutionId, userId);

        var classActivity = await ctx.ClassActivities.AsNoTracking()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        if (classActivity == null) return new ClassActivityNotFound();

        var classesIds = await ctx.ClassStudents.Where(x => x.StudentId == studentId)
            .Select(x => x.ClassId).ToListAsync();
        if (!classesIds.Contains(classActivity.ClassId)) return new StudentNotEnrolledInClass();

        var work = await ctx.ClassActivityWorks.FirstOrDefaultAsync(w => w.ClassActivityId == id && w.StudentId == studentId);
        if (work == null) return ClassActivityWorkNotFound.I;

        work.AddLink(data.Link);

        await ctx.SaveChangesAsync();

        return new CreateClassActivityWorkOut { Id = work.Id };
    }
}
