namespace Estud.Back.Features.Students.GetStudentDetails;

public class GetStudentDetailsService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetStudentDetailsOut, EstudError>> Get(int studentId)
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var student = await ctx.Students.AsNoTracking()
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == studentId && s.InstitutionId == institutionId);
        if (student == null) return StudentNotFound.I;

        var course = await GetCourse(studentId);
        var classes = await GetClasses(studentId, institutionId);

        // Mock: nota e frequência médias aleatórias, porém estáveis por aluno (seed = Id),
        // iguais às exibidas no detalhe da turma.
        // TODO: calcular a partir das notas e presenças reais do aluno.
        var random = new Random(student.Id);
        var averageGrade = Math.Round((decimal)(random.NextDouble() * 10), 1);
        var averageAttendance = Math.Round((decimal)(random.NextDouble() * 100), 1);

        foreach (var @class in classes)
        {
            @class.AverageGrade = averageGrade;
            @class.AverageAttendance = averageAttendance;
        }

        return new GetStudentDetailsOut
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.User!.Email!,
            EnrollmentCode = student.EnrollmentCode,
            Status = student.Status,
            YieldCoefficient = student.YieldCoefficient,
            AverageGrade = averageGrade,
            AverageAttendance = averageAttendance,
            Course = course,
            Classes = classes,
        };
    }

    private async Task<GetStudentDetailsCourseOut?> GetCourse(int studentId)
    {
        return await ctx.StudentCourseEnrollments.AsNoTracking()
            .Where(e => e.StudentId == studentId && e.LeftAt == null)
            .OrderByDescending(e => e.EnrolledAt)
            .Select(e => new GetStudentDetailsCourseOut
            {
                CourseOfferingId = e.CourseOfferingId,
                Course = e.CourseOffering!.Course!.Name,
                Campus = e.CourseOffering.Campus!.Name,
                Period = e.CourseOffering.AcademicPeriod!.Name,
                Session = e.CourseOffering.Session,
                EnrolledAt = e.EnrolledAt,
            })
            .FirstOrDefaultAsync();
    }

    private async Task<List<GetStudentDetailsClassOut>> GetClasses(int studentId, int institutionId)
    {
        var classes = await ctx.ClassStudents.AsNoTracking()
            .Where(cs => cs.StudentId == studentId && cs.Class!.InstitutionId == institutionId)
            .OrderByDescending(cs => cs.Class!.Period.Name)
            .ThenBy(cs => cs.Class!.Discipline.Name)
            .Select(cs => new GetStudentDetailsClassOut
            {
                Id = cs.ClassId,
                Discipline = cs.Class!.Discipline.Name,
                Period = cs.Class.Period.Name,
                Workload = cs.Class.Workload,
                Status = cs.Class.Status,
                MyStatus = cs.Status,
            })
            .ToListAsync();

        // Mesma regra do GetClass: fora de um período de matrícula vigente, uma
        // turma liberada para matrícula é exibida como aguardando início.
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        if (classes.Any(c => c.Status == ClassStatus.OnEnrollment))
        {
            var hasCurrentEnrollmentPeriod = await ctx.EnrollmentPeriods.AsNoTracking()
                .AnyAsync(p => p.InstitutionId == institutionId && p.StartAt <= today && today <= p.EndAt);

            if (!hasCurrentEnrollmentPeriod)
            {
                foreach (var @class in classes.Where(c => c.Status == ClassStatus.OnEnrollment))
                    @class.Status = ClassStatus.OnReview;
            }
        }

        return classes;
    }
}
