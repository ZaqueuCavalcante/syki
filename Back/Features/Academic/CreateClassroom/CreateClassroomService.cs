namespace Syki.Back.Features.Academic.CreateClassroom;

public class CreateClassroomService(SykiDbContext ctx) : IAcademicService
{
    public async Task<OneOf<CreateClassroomOut, SykiError>> Create(Guid institutionId, CreateClassroomIn data)
    {
        var campusOk = await ctx.Campi.AnyAsync(c => c.InstitutionId == institutionId && c.Id == data.CampusId);
        if (!campusOk) return new CampusNotFound();

        var classroom = new Classroom(institutionId, data.CampusId, data.Name, data.Capacity);

        await ctx.SaveChangesAsync(classroom);

        return classroom.ToCreateClassroomOut();
    }
}
