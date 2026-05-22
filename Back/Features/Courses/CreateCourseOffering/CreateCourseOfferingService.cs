using Syki.Back.Domain.Courses;

namespace Syki.Back.Features.Courses.CreateCourseOffering;

public class CreateCourseOfferingService(SykiDbContext ctx) : ISykiService
{
    private class Validator : AbstractValidator<CreateCourseOfferingIn>
    {
        public Validator()
        {
            RuleFor(x => x.CourseSession).NotNull().WithError(InvalidCourseSession.I);
            RuleFor(x => x.CourseSession).IsInEnum().WithError(InvalidCourseSession.I);
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CreateCourseOfferingOut, SykiError>> Create(CreateCourseOfferingIn data)
    {
        if (V.Run(data, out var error)) return error;

        var institutionId = ctx.RequestUser.InstitutionId;

        var campusOk = await ctx.Campi.AnyAsync(x => x.Id == data.CampusId && x.InstitutionId == institutionId);
        if (!campusOk) return CampusNotFound.I;

        var courseOk = await ctx.Courses.AnyAsync(x => x.Id == data.CourseId && x.InstitutionId == institutionId);
        if (!courseOk) return CourseNotFound.I;

        var courseCurriculumOk = await ctx.CourseCurriculums.AnyAsync(x => x.Id == data.CourseCurriculumId && x.CourseId == data.CourseId && x.InstitutionId == institutionId);
        if (!courseCurriculumOk) return CourseCurriculumNotFound.I;

        var academicPeriodOk = await ctx.AcademicPeriods.AnyAsync(x => x.Id == data.AcademicPeriodId && x.InstitutionId == institutionId);
        if (!academicPeriodOk) return AcademicPeriodNotFound.I;

        var courseOffering = new CourseOffering(
            institutionId,
            data.CampusId,
            data.CourseId,
            data.CourseCurriculumId,
            data.AcademicPeriodId,
            data.CourseSession!.Value
        );

        await ctx.SaveChangesAsync(courseOffering);

        return new CreateCourseOfferingOut { Id = courseOffering.Id };
    }
}
