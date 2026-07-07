using Syki.Back.Domain.Classrooms;

namespace Syki.Back.Features.Classrooms.CreateClassroom;

public class CreateClassroomService(SykiDbContext ctx) : ISykiService
{
    public async Task<OneOf<CreateClassroomOut, SykiError>> Create(CreateClassroomIn data)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var campusOk = await ctx.Campi.AnyAsync(c => c.InstitutionId == institutionId && c.Id == data.CampusId);
        if (!campusOk) return CampusNotFound.I;

        var classroom = new Classroom(institutionId, data.CampusId, data.Name, data.Capacity);

        await ctx.SaveChangesAsync(classroom);

        return new CreateClassroomOut { Id = classroom.Id };
    }
}
