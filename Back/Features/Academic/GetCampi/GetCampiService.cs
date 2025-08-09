namespace Syki.Back.Features.Academic.GetCampi;

public class GetCampiService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<CampusOut>> Get(Guid institutionId)
    {
        var campi = await ctx.Campi.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId)
            .OrderBy(c => c.Name)
            .ToListAsync();

        FormattableString sql = $@"
            SELECT
                c.id AS id,
                count(DISTINCT s.id) AS students,
                count(DISTINCT tc.syki_teacher_id) AS teachers
            FROM
            	syki.campi c
            LEFT JOIN
            	syki.teachers__campi tc ON tc.campus_id = c.id
            LEFT JOIN
                syki.course_offerings co ON co.campus_id = c.id
            LEFT JOIN
                syki.students s ON s.course_offering_id = co.id
            WHERE
                c.institution_id = {institutionId}
            GROUP BY
                c.id
        ";
        var totals = await ctx.Database.SqlQuery<CampusEnrollmentDto>(sql).ToListAsync();

        var result = campi.ConvertAll(x =>
        {
            var campusOut = x.ToOut();
            campusOut.Students = totals.FirstOrDefault(t => t.Id == x.Id)?.Students ?? 0;
            campusOut.Teachers = totals.FirstOrDefault(t => t.Id == x.Id)?.Teachers ?? 0;
            campusOut.FillRate = campusOut.Capacity > 0 ? Math.Round(100M * (1M * campusOut.Students / (1M * campusOut.Capacity)), 2) : 0;
            return campusOut;
        });

        return result;
    }

    private class CampusEnrollmentDto
    {
        public Guid Id { get; set; }
        public int Students { get; set; }
        public int Teachers { get; set; }
    }
}
