namespace Syki.Back.Features.Academic.GetCampusCourseOfferings;

public class GetCampusCourseOfferingsService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<GetCampusCourseOfferingsOut>> Get(Guid campusId)
    {
        var courseOfferings = await ctx.CourseOfferings.AsNoTracking()
            .Where(x => x.InstitutionId == ctx.InstitutionId && x.CampusId == campusId)
            .Include(x => x.Course)
            .ToListAsync();

        var ccIds = courseOfferings.ConvertAll(x => x.CourseCurriculumId);
        var ccDisciplines = await ctx.CourseCurriculumDisciplines.AsNoTracking()
            .Where(x => ccIds.Contains(x.CourseCurriculumId))
            .ToListAsync();

        var disciplinesIds = ccDisciplines.ConvertAll(x => x.DisciplineId).Distinct();
        var disciplines = await ctx.Disciplines.Select(x => new { x.Id, x.Name })
            .Where(x => disciplinesIds.Contains(x.Id))
            .ToListAsync();

        var academicPeriod = await ctx.GetCurrentAcademicPeriod(ctx.InstitutionId);

        var result = new List<GetCampusCourseOfferingsOut>();

        foreach (var item in courseOfferings)
        {
            var period = item.GetCurrentPeriodNumber(academicPeriod.Id);
            var disciplinesOut = ccDisciplines
                .Where(x => x.CourseCurriculumId == item.CourseCurriculumId && x.Period == period)
                .Select(x => new GetCampusCourseOfferingsDisciplineOut()
                {
                    Id = x.DisciplineId,
                    Workload = x.Workload,
                    Name = disciplines.First(d => d.Id == x.DisciplineId).Name,
                }).ToList();
            if (disciplinesOut.Count == 0) continue;

            var campusCO = new GetCampusCourseOfferingsOut
            {
                CourseOfferingId = item.Id,
                Course = item.Course.Name,
                Disciplines = disciplinesOut,
            };
            result.Add(campusCO);
        }

        return result;
    }
}
