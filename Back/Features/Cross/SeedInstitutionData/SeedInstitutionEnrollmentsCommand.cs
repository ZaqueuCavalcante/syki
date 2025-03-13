using Syki.Back.Features.Academic.StartClasses;
using Syki.Back.Features.Student.CreateStudentEnrollment;
using Syki.Back.Features.Academic.UpdateEnrollmentPeriod;
using Syki.Back.Features.Academic.CreateEnrollmentPeriod;
using Syki.Back.Features.Academic.ReleaseClassesForEnrollment;

namespace Syki.Back.Features.Cross.SeedInstitutionData;

[CommandDescription("Realizar seed de matrículas da instituição")]
public record SeedInstitutionEnrollmentsCommand(Guid InstitutionId, Guid AdsCourseCurriculumId) : ICommand;

public class SeedInstitutionEnrollmentsCommandHandler(
    SykiDbContext ctx,
    StartClassesService startClassesService,
    CreateEnrollmentPeriodService createEnrollmentPeriodService,
    UpdateEnrollmentPeriodService updateEnrollmentPeriodService,
    CreateStudentEnrollmentService createStudentEnrollmentService,
    ReleaseClassesForEnrollmentService releaseClassesForEnrollmentService) : ICommandHandler<SeedInstitutionEnrollmentsCommand>
{
    public async Task Handle(Guid commandId, SeedInstitutionEnrollmentsCommand command)
    {
        var id = command.InstitutionId;
        var today = DateTime.Now.ToDateOnly();
        var academicPeriod = $"{today.Year}.1";
        var firstDay = new DateTime(today.Year, 1, 1).ToDateOnly();
        var lastDay = new DateTime(today.Year, 12, 31).ToDateOnly();

        await createEnrollmentPeriodService.CreateWithThrowOnError(id, new()
        {
            Id = academicPeriod,
            StartAt = firstDay,
            EndAt = lastDay,
        });

        var classes = await ctx.Classes.Where(x => x.InstitutionId == id).Select(x => x.Id).ToListAsync();
        await releaseClassesForEnrollmentService.ReleaseWithThrowOnError(id, new() { Classes = classes });

        var adsCourseOfferingId = await ctx.CourseOfferings
            .Where(x => x.InstitutionId == id && x.CourseCurriculumId == command.AdsCourseCurriculumId)
            .Select(x => x.Id).FirstAsync();

        var students = await ctx.Students.Where(x => x.CourseOfferingId == adsCourseOfferingId).Select(x => new { x.Id }).ToListAsync();
        foreach (var student in students)
        {
            await createStudentEnrollmentService.CreateWithThrowOnError(id, student.Id, command.AdsCourseCurriculumId, new() { Classes = classes });
        }

        await updateEnrollmentPeriodService.UpdateWithThrowOnError(id, academicPeriod, new() { StartAt = new DateTime(today.Year, 3, 3).ToDateOnly(), EndAt = new DateTime(today.Year, 3, 7).ToDateOnly() });

        await startClassesService.StartWithThrowOnError(id, new() { Classes = classes });

        ctx.AddCommand(id, new SeedInstitutionLessonAttendancesCommand(id), parentId: commandId);
    }
}
