namespace Estud.Back.Features.Teachers.GetTeacherClassStudents;

public class GetTeacherClassStudentsService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetTeacherClassStudentsOut, EstudError>> Get(int classId)
    {
        var userId = ctx.RequestUser.Id;
        var institutionId = ctx.RequestUser.InstitutionId;
        var teacherId = await ctx.GetTeacherId(institutionId, userId);

        var @class = await ctx.Classes.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == classId && c.InstitutionId == institutionId);
        if (@class == null) return ClassNotFound.I;

        var assigned = await ctx.ClassTeachers.AnyAsync(ct => ct.ClassId == classId && ct.TeacherId == teacherId);
        if (!assigned) return TeacherNotAssignedToClass.I;

        var classStudents = await ctx.ClassStudents.AsNoTracking()
            .Where(cs => cs.ClassId == classId)
            .OrderBy(cs => cs.Student!.Name)
            .Select(cs => new
            {
                Id = cs.StudentId,
                cs.Student!.Name,
                cs.Status,
            })
            .ToListAsync();

        // Mock: nota e frequência médias aleatórias, porém estáveis por aluno (seed = Id).
        // TODO: calcular a partir das notas e presenças reais do aluno na turma.
        var students = classStudents
            .Select(s =>
            {
                var random = new Random(s.Id);
                return new GetTeacherClassStudentsItemOut
                {
                    Id = s.Id,
                    Name = s.Name,
                    Status = s.Status,
                    AverageGrade = Math.Round((decimal)(random.NextDouble() * 10), 1),
                    AverageAttendance = Math.Round((decimal)(random.NextDouble() * 100), 1),
                };
            })
            .ToList();

        return new GetTeacherClassStudentsOut { Students = students };
    }
}
