namespace Syki.Back.Features.Academic.GetCampiEnrollment;

public class GetCampiEnrollmentService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<GetCampusEnrollmentOut>> Get(Guid institutionId)
    {
        var campi = await ctx.Campi.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId)
            .OrderBy(c => c.Name)
            .ToListAsync();

        FormattableString sql = $@"
            SELECT
                co.campus_id AS id,
                count(s.id)  AS students
            FROM
                syki.course_offerings co
            INNER JOIN
                syki.students s ON s.course_offering_id = co.id
            GROUP BY
                co.campus_id;
        ";
        var totals = await ctx.Database.SqlQuery<CampusEnrollmentDto>(sql).ToListAsync();

        var result = campi.ConvertAll(x =>
        {
            var campusOut = x.ToEnrollmentOut();
            campusOut.Students = totals.FirstOrDefault(t => t.Id == x.Id)?.Students ?? 0;
            campusOut.FillRate = campusOut.Capacity > 0 ? Math.Round(100M * (1M * campusOut.Students / (1M * campusOut.Capacity)), 2) : 0;
            return campusOut;
        });

        return result;
    }

    private class CampusEnrollmentDto
    {
        public Guid Id { get; set; }
        public int Students { get; set; }
    }
}
