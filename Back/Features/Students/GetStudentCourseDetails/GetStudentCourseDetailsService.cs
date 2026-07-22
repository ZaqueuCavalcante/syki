namespace Estud.Back.Features.Students.GetStudentCourseDetails;

public class GetStudentCourseDetailsService(EstudDbContext ctx) : IEstudService
{
    public async Task<OneOf<GetStudentCourseDetailsOut, EstudError>> Get()
    {
        var userId = ctx.RequestUser.Id;
        var institutionId = ctx.RequestUser.InstitutionId;
        var studentId = await ctx.GetStudentId(institutionId, userId);
        if (studentId == 0) return StudentNotFound.I;

        // Oferta de curso atual do aluno (vínculo ativo mais recente)
        var enrollment = await ctx.StudentCourseEnrollments.AsNoTracking()
            .Where(e => e.StudentId == studentId && e.LeftAt == null)
            .OrderByDescending(e => e.EnrolledAt)
            .Select(e => new
            {
                e.CourseOfferingId,
                e.CourseOffering!.CourseCurriculumId,
                Course = e.CourseOffering.Course!.Name,
                Curriculum = e.CourseOffering.CourseCurriculum!.Name,
                Campus = e.CourseOffering.Campus!.Name,
                Period = e.CourseOffering.AcademicPeriod!.Name,
                e.CourseOffering.Session,
            })
            .FirstOrDefaultAsync();
        if (enrollment == null) return StudentNotEnrolledInAnyCourse.I;

        // Grade de disciplinas da oferta
        var links = await ctx.CourseCurriculumDisciplines.AsNoTracking()
            .Where(l => l.CourseCurriculumId == enrollment.CourseCurriculumId)
            .ToListAsync();

        var disciplineIds = links.Select(l => l.DisciplineId).ToList();

        var disciplineNames = await ctx.Disciplines.AsNoTracking()
            .Where(d => disciplineIds.Contains(d.Id))
            .Select(d => new { d.Id, d.Name })
            .ToListAsync();
        var nameMap = disciplineNames.ToDictionary(d => d.Id, d => d.Name);

        // Status do aluno em cada disciplina, com base nas turmas em que ele está/esteve vinculado
        var classStatuses = await ctx.ClassStudents.AsNoTracking()
            .Where(cs => cs.StudentId == studentId
                && cs.Class!.InstitutionId == institutionId
                && disciplineIds.Contains(cs.Class.DisciplineId))
            .Select(cs => new { cs.Class!.DisciplineId, cs.Status })
            .ToListAsync();

        var statusMap = classStatuses
            .GroupBy(x => x.DisciplineId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Status).ToBestStudentDisciplineStatus());

        var disciplines = links
            .OrderBy(l => l.Period)
            .ThenBy(l => nameMap.GetValueOrDefault(l.DisciplineId, string.Empty))
            .Select(l => new GetStudentCourseDetailsDisciplineOut
            {
                Id = l.DisciplineId,
                Name = nameMap.GetValueOrDefault(l.DisciplineId, string.Empty),
                Period = l.Period,
                Credits = l.Credits,
                Workload = l.Workload,
                Status = statusMap.GetValueOrDefault(l.DisciplineId, StudentDisciplineStatus.NaoCursada),
            })
            .ToList();

        return new GetStudentCourseDetailsOut
        {
            CourseOfferingId = enrollment.CourseOfferingId,
            Course = enrollment.Course,
            Curriculum = enrollment.Curriculum,
            Campus = enrollment.Campus,
            Period = enrollment.Period,
            Session = enrollment.Session,
            Disciplines = disciplines,
        };
    }
}
