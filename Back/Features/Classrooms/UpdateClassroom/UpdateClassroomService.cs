namespace Estud.Back.Features.Classrooms.UpdateClassroom;

public class UpdateClassroomService(EstudDbContext ctx) : IEstudService
{
    private class Validator : AbstractValidator<UpdateClassroomIn>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithError(InvalidClassroomName.I);
            RuleFor(x => x.Name).MaximumLength(50).WithError(InvalidClassroomName.I);

            RuleFor(x => x.Capacity).GreaterThan(0).WithError(InvalidClassroomCapacity.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<UpdateClassroomOut, EstudError>> Update(UpdateClassroomIn data)
    {
        if (V.Run(data, out var error)) return error;

        var institutionId = ctx.RequestUser.InstitutionId;

        var classroom = await ctx.Classrooms.FirstOrDefaultAsync(x => x.InstitutionId == institutionId && x.Id == data.Id);
        if (classroom == null) return ClassroomNotFound.I;

        classroom.Update(data.Name, data.Capacity);

        await ctx.SaveChangesAsync();

        return classroom.ToUpdateClassroomOut();
    }
}
