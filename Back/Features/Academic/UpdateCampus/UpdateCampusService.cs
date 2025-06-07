namespace Syki.Back.Features.Academic.UpdateCampus;

public class UpdateCampusService(SykiDbContext ctx, HybridCache cache) : IAcademicService
{
	public async Task<OneOf<CampusOut, SykiError>> Update(Guid institutionId, UpdateCampusIn data)
	{
		var campus = await ctx.Campi.FirstOrDefaultAsync(x => x.InstitutionId == institutionId && x.Id == data.Id);
		if (campus == null) return new CampusNotFound();

		campus.Update(data.Name, data.State, data.City);

		await ctx.SaveChangesAsync();
		await cache.RemoveAsync($"campi:{institutionId}");

		return campus.ToOut();
	}
}
