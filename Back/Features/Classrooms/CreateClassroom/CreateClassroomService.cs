using Estud.Back.Domain.Classrooms;

namespace Estud.Back.Features.Classrooms.CreateClassroom;

public class CreateClassroomService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<CreateClassroomOut, EstudError>> Create(CreateClassroomIn data)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var campusOk = await ctx.Campi.AnyAsync(c => c.InstitutionId == institutionId && c.Id == data.CampusId);
        if (!campusOk) return CampusNotFound.I;

        var classroom = new Classroom(institutionId, data.CampusId, data.Name, data.Capacity);
        await ctx.SaveChangesAsync(classroom);

        return new CreateClassroomOut { Id = classroom.Id };
    }
}
